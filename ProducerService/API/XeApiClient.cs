using Newtonsoft.Json.Linq;
using SharedUtils.ModelEntities;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProducerService.API
{
    public class XeApiClient : ICurrencyApiClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string BaseUrl = ConfigurationManager.AppSettings["XEBaseUrl"];
        private readonly string _accountId;
        private readonly string _apiKey;

        public XeApiClient()
        {
            _accountId = ConfigurationManager.AppSettings["AccountApiId"];
            _apiKey = ConfigurationManager.AppSettings["ApiKey"];
            var authToken = Encoding.ASCII.GetBytes($"{_accountId}:{_apiKey}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
        }

        public async Task<CurrencyPair> GetExchangeRatesAsync(string baseCurrency, string targetCurrency)
        {
            try
            {
                var requestUrl = $"{BaseUrl}?from={baseCurrency}&to={targetCurrency}&amount=1&random=12345";

                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JObject.Parse(jsonResponse);

                var rate = parsedResponse["to"].FirstOrDefault(r => r["quotecurrency"].ToString() == targetCurrency);

                return new CurrencyPair
                {
                    PairName = $"{baseCurrency}/{targetCurrency}",
                    Value = Convert.ToDecimal(rate["mid"]),
                    LastModifiedDate = DateTime.UtcNow
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching from Xe API: {ex.Message}");
                throw; //will allow the fallback
            }
        }
    }
}
