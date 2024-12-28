using SharedUtils.ModelEntities;
using System;
using System.Threading.Tasks;

namespace ProducerService.API
{
    //Bonus Question class
    public class CurrencyApiClientWithFallback : ICurrencyApiClient
    {
        private readonly ICurrencyApiClient _primaryClient;
        private readonly ICurrencyApiClient _secondaryClient;

        public CurrencyApiClientWithFallback(ICurrencyApiClient primaryClient, ICurrencyApiClient secondaryClient)
        {
            _primaryClient = primaryClient;
            _secondaryClient = secondaryClient;
        }

        public async Task<CurrencyPair> GetExchangeRatesAsync(string baseCurrency, string targetCurrency)
        {
            try
            {
                // Try the primary client (OpenExchangeApiClient)
                return await _primaryClient.GetExchangeRatesAsync(baseCurrency, targetCurrency);
            }
            catch (Exception ex)
            {
                // Log or handle the primary client failure
                Console.WriteLine($"Error fetching from primary source (OpenExchangeApiClient): {ex.Message}");

                // If primary client fails, fall back to the secondary client (XeApiClient)
                Console.WriteLine("Falling back to secondary source (XeApiClient)...");
                return await _secondaryClient.GetExchangeRatesAsync(baseCurrency, targetCurrency);
            }
        }
    }
}
