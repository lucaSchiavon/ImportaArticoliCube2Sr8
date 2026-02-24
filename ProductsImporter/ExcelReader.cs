using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.InteropServices;

namespace ProductsImporter
{
    /// <summary>
    /// Legge semplicemente un file Excel .xlsx senza usare OLEDB.
    /// Implementazione minimale che tratta il file come un archivio OpenXML,
    /// legge le sharedStrings e la prima worksheet e la scorre riga per riga.
    /// Se viene passato un .csv lo legge come testo separato da virgola/;.
    /// La prima riga è considerata header e viene ignorata nella restituzione.
    /// </summary>
    public class ExcelReader
    {
        public async Task<List<ProductRow>> ReadAsync(string filePath)
        {
            return await Task.Run(() => Read(filePath));
        }

        private void MarshalRelease(object comObj)
        {
            try
            {
                if (comObj == null) return;
                Marshal.FinalReleaseComObject(comObj);
            }
            catch
            {
                try { Marshal.ReleaseComObject(comObj); } catch { }
            }
        }

        private List<ProductRow> Read(string filePath)
        {
            var rows = new List<ProductRow>();
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return rows;

            var ext = Path.GetExtension(filePath)?.ToLowerInvariant();
            if (ext == ".csv")
            {
                ReadCsv(filePath, rows);
                return rows;
            }

            if (ext != ".xlsx" && ext != ".xls")
            {
                // unsupported
                return rows;
            }

            // Use Excel COM automation (late binding) to read the workbook without OLEDB.
            // This requires Excel to be installed on the machine. It avoids adding Interop assemblies
            // by using dynamic and Activator.CreateInstance.
            object excelApp = null;
            object workbook = null;
            try
            {
                var prog = Type.GetTypeFromProgID("Excel.Application");
                if (prog == null) return rows; // Excel not available
                excelApp = Activator.CreateInstance(prog);
                dynamic app = excelApp;
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(Path.GetFullPath(filePath), ReadOnly: true);
                dynamic wb = workbook;
                dynamic sheet = null;
                try { sheet = app.ActiveSheet; } catch { }
                if (sheet == null)
                    sheet = wb.Worksheets[1];
                dynamic used = sheet.UsedRange;
                int rowCount = (int)used.Rows.Count;
                int colCount = (int)used.Columns.Count;

                // read header
                var headerMap = new Dictionary<int, string>();
                for (int c = 1; c <= colCount; c++)
                {
                    try
                    {
                        var v = used.Cells[1, c].Value2;
                        if (v != null)
                        {
                            headerMap[c] = v.ToString().Trim();
                        }
                    }
                    catch { }
                }

                int colCod = headerMap.FirstOrDefault(kv => string.Equals(kv.Value, "codart", StringComparison.OrdinalIgnoreCase)).Key;
                int colDescr = headerMap.FirstOrDefault(kv => string.Equals(kv.Value, "descr", StringComparison.OrdinalIgnoreCase)).Key;
                int colPrez = headerMap.FirstOrDefault(kv => string.Equals(kv.Value, "prezzo", StringComparison.OrdinalIgnoreCase)).Key;

                for (int r = 2; r <= rowCount; r++)
                {
                    try
                    {
                        string codart = null, descr = null;
                        decimal prezzo = 0;
                        if (colCod > 0)
                        {
                            var cv = used.Cells[r, colCod].Value2;
                            codart = cv?.ToString();
                        }
                        if (string.IsNullOrWhiteSpace(codart))
                            continue;
                        if (colDescr > 0)
                        {
                            var dv = used.Cells[r, colDescr].Value2;
                            descr = dv?.ToString();
                        }
                        if (colPrez > 0)
                        {
                            var pv = used.Cells[r, colPrez].Value2;
                            if (pv != null)
                            {
                                decimal.TryParse(pv.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out prezzo);
                            }
                        }

                        rows.Add(new ProductRow { Codart = codart.Trim(), Descr = descr?.Trim(), Prezzo = prezzo });
                    }
                    catch
                    {
                        // ignore row errors
                    }
                }
            }
            catch
            {
                // if automation failed, fallback to empty list
            }
            finally
            {
                try
                {
                    if (workbook != null)
                    {
                        dynamic wb = workbook;
                        wb.Close(false);
                    }
                }
                catch { }
                try
                {
                    if (excelApp != null)
                    {
                        dynamic app = excelApp;
                        app.Quit();
                    }
                }
                catch { }
                // release COM objects if present
                if (workbook != null) MarshalRelease(workbook);
                if (excelApp != null) MarshalRelease(excelApp);
            }

            return rows;
        }

        private static string Normalize(string s)
        {
            return Regex.Replace(s ?? string.Empty, "[^a-zA-Z0-9]", string.Empty).ToLowerInvariant();
        }

        private static string ExtractColumnLetters(string cellRef)
        {
            if (string.IsNullOrEmpty(cellRef)) return null;
            var m = Regex.Match(cellRef.ToUpperInvariant(), "^([A-Z]+)");
            return m.Success ? m.Groups[1].Value : null;
        }

        private static string ReadCellValue(XElement cell, List<string> shared)
        {
            if (cell == null) return null;
            var t = (string)cell.Attribute("t");
            var v = cell.Elements().FirstOrDefault(x => x.Name.LocalName == "v")?.Value;
            if (t == "s")
            {
                if (int.TryParse(v, out var idx) && idx >= 0 && idx < shared.Count)
                    return shared[idx];
                return v;
            }
            if (t == "inlineStr")
            {
                var isNode = cell.Elements().FirstOrDefault(x => x.Name.LocalName == "is");
                var tNode = isNode?.Descendants().FirstOrDefault(x => x.Name.LocalName == "t");
                return tNode?.Value;
            }
            return v;
        }

        private void ReadCsv(string filePath, List<ProductRow> rows)
        {
            var all = File.ReadAllLines(filePath);
            if (all.Length <= 1) return;
            var header = all[0].Split(new[] { ';', ',' });
            int idxCod = Array.FindIndex(header, h => string.Equals(h.Trim(), "codart", StringComparison.OrdinalIgnoreCase));
            int idxDescr = Array.FindIndex(header, h => string.Equals(h.Trim(), "descr", StringComparison.OrdinalIgnoreCase));
            int idxPrez = Array.FindIndex(header, h => string.Equals(h.Trim(), "prezzo", StringComparison.OrdinalIgnoreCase));
            for (int i = 1; i < all.Length; i++)
            {
                var parts = all[i].Split(new[] { ';', ',' });
                if (idxCod >= 0 && idxCod < parts.Length)
                {
                    var cod = parts[idxCod].Trim();
                    if (string.IsNullOrWhiteSpace(cod)) continue;
                    var descr = (idxDescr >= 0 && idxDescr < parts.Length) ? parts[idxDescr].Trim() : null;
                    var prezStr = (idxPrez >= 0 && idxPrez < parts.Length) ? parts[idxPrez].Trim() : null;
                    decimal prezzo = 0;
                    if (!string.IsNullOrWhiteSpace(prezStr))
                    {
                        if (!decimal.TryParse(prezStr, NumberStyles.Any, CultureInfo.InvariantCulture, out prezzo))
                            decimal.TryParse(prezStr, NumberStyles.Any, CultureInfo.CurrentCulture, out prezzo);
                    }
                    rows.Add(new ProductRow { Codart = cod, Descr = descr, Prezzo = prezzo });
                }
            }
        }
    }
}
