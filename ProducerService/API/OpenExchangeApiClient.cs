using Newtonsoft.Json.Linq;
using SharedUtils.ModelEntities;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProducerService.API
{
    public class OpenExchangeApiClient : ICurrencyApiClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string BaseUrl = ConfigurationManager.AppSettings["OpenExchangeBaseUrl"];
        private readonly string _app_id;

        public OpenExchangeApiClient()
        {
            _app_id = ConfigurationManager.AppSettings["app_id"];
        }

        public async Task<CurrencyPair> GetExchangeRatesAsync(string baseCurrency, string targetCurrency)
        {
            // Logic for fetching data from Open Exchange API
            var requestUrl = $"{BaseUrl}?app_id={_app_id}&base={baseCurrency}&symbols={targetCurrency}";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var parsedResponse = JObject.Parse(jsonResponse);

            return new CurrencyPair
            {
                PairName = $"{baseCurrency}/{targetCurrency}",
                Value = Convert.ToDecimal(parsedResponse["rates"][targetCurrency]),
                LastModifiedDate = DateTime.UtcNow
            };
        }
    }
}
