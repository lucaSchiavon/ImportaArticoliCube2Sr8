using System;
using System.IO;
using System.Xml;

namespace ProductsImporter
{
    public static class ConnectionStringProvider
    {
        public static string GetConnectionString(string name)
        {
            try
            {
                var configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                if (!File.Exists(configFile))
                    return null;
                var doc = new XmlDocument();
                doc.Load(configFile);
                var node = doc.SelectSingleNode($"/configuration/connectionStrings/add[@name='{name}']");
                if (node?.Attributes == null) return null;
                var attr = node.Attributes["connectionString"];
                return attr?.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
