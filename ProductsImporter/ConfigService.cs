using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ProductsImporter
{
    public class ConfigService
    {
        private readonly string _path;
        public ConfigRoot Config { get; private set; }

        public ConfigService(string path = null)
        {
            _path = path ?? AppDomain.CurrentDomain.BaseDirectory;
        }

        public void Load()
        {
            var file = Path.Combine(_path, "config.json");
            if (!File.Exists(file))
            {
                // create default
                Config = new ConfigRoot
                {
                    DefaultValues = new DefaultsConfig
                    {
                        lc_conto = 0,
                        lc_listino = 100,
                        ar_unmis = "PZ",
                        ar_codiva = "31",
                        codditt = "DEFAULT"
                    }
                };
                Save();
                return;
            }

            var txt = File.ReadAllText(file);
            // very small and forgiving JSON parser for the expected structure
            Config = new ConfigRoot { DefaultValues = new DefaultsConfig() };
            Config.DefaultValues.lc_conto = ReadInt(txt, "lc_conto", 0);
            Config.DefaultValues.lc_listino = (short)ReadInt(txt, "lc_listino", 100);
            Config.DefaultValues.ar_unmis = ReadString(txt, "ar_unmis", "PZ");
            Config.DefaultValues.ar_codiva = ReadString(txt, "ar_codiva", "31");
            Config.DefaultValues.codditt = ReadString(txt, "codditt", "DEFAULT");
        }

        public void Save()
        {
            var file = Path.Combine(_path, "config.json");
            var txt = "{\r\n  \"DefaultValues\": {\r\n    \"lc_conto\": " + Config.DefaultValues.lc_conto + ",\r\n    \"lc_listino\": " + Config.DefaultValues.lc_listino + ",\r\n    \"ar_unmis\": \"" + Config.DefaultValues.ar_unmis + "\",\r\n    \"ar_codiva\": \"" + Config.DefaultValues.ar_codiva + "\",\r\n    \"codditt\": \"" + Config.DefaultValues.codditt + "\"\r\n  }\r\n}";
            File.WriteAllText(file, txt);
        }

        private int ReadInt(string txt, string key, int def)
        {
            var m = Regex.Match(txt, "\"" + key + "\"\\s*:\\s*(\\d+)");
            if (m.Success && int.TryParse(m.Groups[1].Value, out var v))
                return v;
            return def;
        }

        private string ReadString(string txt, string key, string def)
        {
            var m = Regex.Match(txt, "\"" + key + "\"\\s*:\\s*\"([^\"]*)\"");
            if (m.Success)
                return m.Groups[1].Value;
            return def;
        }
    }
}
