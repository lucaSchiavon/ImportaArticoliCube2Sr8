using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;

namespace ProductsImporter
{
    public class ListiniRepository
    {
        private readonly string _connString;

        public ListiniRepository(string connString = null)
        {
            _connString = connString ?? ConnectionStringProvider.GetConnectionString("DefaultConnection");
        }

        public async Task InsertOrUpdateAsync(string codditt, string codart, int lc_conto, short lc_listino, decimal prezzo)
        {
            string DataIns = DateTime.Now.ToString("yyyyMMdd");
            string DataOraIns = DateTime.Now.ToString("yyyyMMdd HH:MM:ss");
            var existsSql = "SELECT COUNT(1) FROM dbo.listini WHERE codditt=@codditt AND lc_codart=@codart AND lc_conto=@conto AND lc_listino=@listino";
            var insertSql = "INSERT INTO listini (codditt, lc_codart, lc_codlavo, lc_conto, lc_codvalu, lc_codtpro, lc_listino, lc_datagg, lc_tipo, lc_prezzo, lc_datscad, lc_daquant, lc_aquant, lc_perqta, lc_unmis, lc_note, lc_netto, lc_fase, lc_ultagg, lc_codcas, lc_coddest ) " +
                "VALUES  ( @codditt, @codart, 0, 0, 0, 0, 100, @datains, ' ', @prezzo, '20991231', 0, 9999999999, 1, 'PZ', null, 'N', 0, @dataorains, ' ', 0 )";
            //var insertSql = "INSERT INTO listini (codditt, lc_codart, lc_codlavo, lc_conto, lc_codvalu, lc_codtpro, lc_listino, lc_datagg, lc_tipo, lc_prezzo, lc_datscad, lc_daquant, lc_aquant, lc_perqta, lc_unmis, lc_note, lc_netto, lc_fase, lc_ultagg, lc_codcas, lc_coddest ) " +
            //   "VALUES  ( @codditt, @codart, 0, 0, 0, 0, 100, @datins', ' ', @prezzo, '20991231', 0, 9999999999, 1, 'PZ', null, 'N', 0, '20260220 17:52:38', ' ', 0 )";
            //var insertSql = "INSERT INTO dbo.listini (codditt, lc_codart, lc_conto, lc_listino, lc_prezzo) VALUES (@codditt,@codart,@conto,@listino,@prezzo)";
            var updateSql = "UPDATE dbo.listini SET lc_prezzo=@prezzo WHERE codditt=@codditt AND lc_codart=@codart AND lc_conto=@conto AND lc_listino=@listino";

            using (var cn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = existsSql;
                cmd.Parameters.AddWithValue("@codditt", codditt);
                cmd.Parameters.AddWithValue("@codart", codart);
                cmd.Parameters.AddWithValue("@conto", lc_conto);
                cmd.Parameters.AddWithValue("@listino", lc_listino);
                await cn.OpenAsync();
                var exists = Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;

                cmd.Parameters.Clear();
                if (exists)
                {
                    cmd.CommandText = updateSql;
                    cmd.Parameters.AddWithValue("@prezzo", prezzo);
                    cmd.Parameters.AddWithValue("@codditt", codditt);
                    cmd.Parameters.AddWithValue("@codart", codart);
                    cmd.Parameters.AddWithValue("@conto", lc_conto);
                    cmd.Parameters.AddWithValue("@listino", lc_listino);
                    //cmd.Parameters.AddWithValue("@datains", DataIns);
                    //cmd.Parameters.AddWithValue("@dataorains", DataOraIns);
                    await cmd.ExecuteNonQueryAsync();
                }
                else
                {
                    cmd.CommandText = insertSql;
                    cmd.Parameters.AddWithValue("@prezzo", prezzo);
                    cmd.Parameters.AddWithValue("@codditt", codditt);
                    cmd.Parameters.AddWithValue("@codart", codart);
                    cmd.Parameters.AddWithValue("@conto", lc_conto);
                    cmd.Parameters.AddWithValue("@listino", lc_listino);
                    cmd.Parameters.AddWithValue("@datains", DataIns);
                    cmd.Parameters.AddWithValue("@dataorains", DataOraIns);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
