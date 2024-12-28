using SharedUtils.ModelEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ProducerService.Services
{
    public class ExchangeRateFetcher
    {
        private readonly ICurrencyApiClient _currencyApiClient;
        private readonly Repository _repository;
        private readonly BroadcastBlock<List<CurrencyPair>> _broadcastBlock;

        public ExchangeRateFetcher(ICurrencyApiClient currencyApiClient, Repository repository, BroadcastBlock<List<CurrencyPair>> broadcastBlock)
        {
            _currencyApiClient = currencyApiClient;
            _repository = repository;
            _broadcastBlock = broadcastBlock;
        }

        public async Task FetchAndStoreDataAsync()
        {
            string[] pairs = ConfigurationManager.AppSettings["CurrencyPairs"]
                                    .Split(',')
                                    .Select(pair => pair.Trim())
                                    .ToArray();

            List<CurrencyPair> currencyPairs = new List<CurrencyPair>();

            foreach (var pair in pairs)
            {
                var currencies = pair.Split('/');
                var baseCurrency = currencies[0];
                var targetCurrency = currencies[1];

                // Fetch the exchange rates for the specific pair
                var fetchedCurrencyPair = await _currencyApiClient.GetExchangeRatesAsync(baseCurrency, targetCurrency);

                // Add the fetched pairs to the list
                currencyPairs.Add(fetchedCurrencyPair);
            }

            if (currencyPairs.Count == 0)
            {
                Console.WriteLine("No available currency pairs.");
                return;
            }

            // Publish data to the BroadcastBlock for consumers
            _broadcastBlock.Post(currencyPairs);

            // Save data to the database
            bool success = await _repository.SaveRates(currencyPairs);

            if (success)
            {
                Console.WriteLine("Currency rates successfully saved to the database.");
            }
            else
            {
                Console.WriteLine("Failed to save currency rates to the database.");
            }
        }
    }
}
