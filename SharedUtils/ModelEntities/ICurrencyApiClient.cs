using System.Threading.Tasks;

namespace SharedUtils.ModelEntities
{
    public interface ICurrencyApiClient
    {
        Task<CurrencyPair> GetExchangeRatesAsync(string baseCurrency, string targetCurrencies);
    }
}
