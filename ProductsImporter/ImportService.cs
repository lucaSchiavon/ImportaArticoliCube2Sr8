using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsImporter
{
    public class ImportService
    {
        private readonly ExcelReader _excelReader;
        private readonly ArticoliRepository _artRepo;
        private readonly ListiniRepository _listRepo;
        private readonly ConfigService _config;
        private readonly LoggerService _logger;

        public ImportService(ExcelReader excelReader, ArticoliRepository artRepo, ListiniRepository listRepo, ConfigService config, LoggerService logger)
        {
            _excelReader = excelReader;
            _artRepo = artRepo;
            _listRepo = listRepo;
            _config = config;
            _logger = logger;
        }

        public async Task<ImportResult> ImportAsync(string filePath, bool creaArticoli, bool importaPrezzi, IProgress<int> progress)
        {
            var result = new ImportResult();
            List<ProductRow> rows;
            try
            {
                rows = await _excelReader.ReadAsync(filePath);
            }
            catch (Exception ex)
            {
                _logger.Log($"Errore lettura file: {ex.Message}");
                result.ErrorDetails.Add("Errore lettura file: " + ex.Message);
                result.Errors = 1;
                return result;
            }

            result.TotalRows = rows.Count;
            int idx = 0;
            foreach (var r in rows)
            {
                idx++;
                try
                {
                    var exists = await _artRepo.ExistsAsync(_config.Config.DefaultValues.codditt, r.Codart);
                    if (!exists)
                    {
                        if (creaArticoli)
                        {
                            await _artRepo.InsertAsync(_config.Config.DefaultValues.codditt, r.Codart, r.Descr, _config.Config.DefaultValues.ar_unmis, _config.Config.DefaultValues.ar_codiva);
                            await _listRepo.InsertOrUpdateAsync(_config.Config.DefaultValues.codditt, r.Codart, _config.Config.DefaultValues.lc_conto, _config.Config.DefaultValues.lc_listino, r.Prezzo);
                            result.Inserted++;
                            _logger.Log($"Inserito articolo {r.Codart}");
                        }
                        else
                        {
                            _logger.Log($"Articolo non trovato e non creato: {r.Codart}");
                        }
                    }
                    else
                    {
                        if (importaPrezzi)
                        {
                            await _listRepo.InsertOrUpdateAsync(_config.Config.DefaultValues.codditt, r.Codart, _config.Config.DefaultValues.lc_conto, _config.Config.DefaultValues.lc_listino, r.Prezzo);
                            result.Updated++;
                            _logger.Log($"Aggiornato prezzo {r.Codart} -> {r.Prezzo}");
                        }
                        else
                        {
                            _logger.Log($"Articolo esistente, prezzo non aggiornato: {r.Codart}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Errors++;
                    var msg = $"Errore elaborando {r.Codart}: {ex.Message}";
                    result.ErrorDetails.Add(msg);
                    _logger.Log(msg);
                }

                progress?.Report((int)((idx * 100.0) / Math.Max(1, result.TotalRows)));
            }

            return result;
        }
    }
}
