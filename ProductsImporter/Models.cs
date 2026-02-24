using System.Collections.Generic;

namespace ProductsImporter
{
    public class DefaultsConfig
    {
        public int lc_conto { get; set; }
        public short lc_listino { get; set; }
        public string ar_unmis { get; set; }
        public string ar_codiva { get; set; }
        public string codditt { get; set; }
    }

    public class ConfigRoot
    {
        public DefaultsConfig DefaultValues { get; set; }
    }

    public class ProductRow
    {
        public string Codart { get; set; }
        public string Descr { get; set; }
        public decimal Prezzo { get; set; }
    }

    public class ImportResult
    {
        public int TotalRows { get; set; }
        public int Inserted { get; set; }
        public int Updated { get; set; }
        public int Errors { get; set; }
        public List<string> ErrorDetails { get; set; } = new List<string>();
    }
}
