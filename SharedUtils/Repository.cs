using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using System.Data;

namespace SharedUtils.ModelEntities
{
    public class Repository
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["CurrencyExchange"].ConnectionString;

        public async Task<bool> SaveRates(List<CurrencyPair> currencyPairs)
        {
            int rowAffected = 0;
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    foreach (var pair in currencyPairs)
                    {
                        var command = new SqlCommand("SaveRates", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PairName", pair.PairName);
                        command.Parameters.AddWithValue("@Value", pair.Value);
                        command.Parameters.AddWithValue("@UpdatedDate", pair.LastModifiedDate);

                        rowAffected = await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured on SaveRates procedure: {ex.Message} - {ex.StackTrace}");
            }
            return rowAffected > 0;
        }

        public List<CurrencyPair> GetCurrencyRatesFromDB()
        {
            var currencyPairs = new List<CurrencyPair>();

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("GetCurrencyRates", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            currencyPairs.Add(new CurrencyPair
                            {
                                PairName = reader["PairName"].ToString(),
                                Value = decimal.Parse(reader["PairValue"].ToString()),
                                LastModifiedDate = DateTime.Parse(reader["UpdatedDate"].ToString())
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while fetching data from DB: {ex.Message} - {ex.StackTrace}");
            }

            return currencyPairs;
        }
    }
}
