using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;

namespace ProductsImporter
{
    public class ArticoliRepository
    {
        private readonly string _connString;

        public ArticoliRepository(string connString = null)
        {
            _connString = connString ?? ConnectionStringProvider.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> ExistsAsync(string codditt, string codart)
        {
            var sql = "SELECT COUNT(1) FROM dbo.artico WHERE codditt=@codditt AND ar_codart=@codart";
            using (var cn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@codditt", codditt);
                cmd.Parameters.AddWithValue("@codart", codart);
                await cn.OpenAsync();
                var o = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(o) > 0;
            }
        }

        public async Task InsertAsync(string codditt, string codart, string descr, string unmis, string codiva)
        {
            //INSERT INTO artico (codditt, ar_codart, ar_descr, ar_unmis, ar_datins, ar_datini, ar_datfin)  VALUES  ( 'EMMEBISRL', 'F1', 'Prodotto F1', 'PZ', '20260220', '19000101', '20991231');
           string DataIns = DateTime.Now.ToString("yyyyMMdd");
            var sql = "INSERT INTO dbo.artico (codditt, ar_codart, ar_descr,ar_datins, ar_datini, ar_datfin, ar_unmis, ar_codiva) VALUES (@codditt, @codart, @descr, @datins ,@datini ,@datfin ,@unmis, @codiva)";
            using (var cn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@codditt", codditt);
                cmd.Parameters.AddWithValue("@codart", codart);
                cmd.Parameters.AddWithValue("@descr", descr ?? string.Empty);
                cmd.Parameters.AddWithValue("@datins", DataIns);
                cmd.Parameters.AddWithValue("@datini", "19000101" ?? string.Empty);
                cmd.Parameters.AddWithValue("@datfin", "20991231" ?? string.Empty);
                cmd.Parameters.AddWithValue("@unmis", unmis ?? string.Empty);
                cmd.Parameters.AddWithValue("@codiva", codiva ?? string.Empty);
                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
