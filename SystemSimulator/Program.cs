using ConsumerService.Services;
using ProducerService.API;
using ProducerService.Services;
using SharedUtils.ModelEntities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SystemSimulator
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var broadcastBlock = new BroadcastBlock<List<CurrencyPair>>(null);
            var repository = new Repository();

            // Initialize CurrencyApi's
            var xeApiClient = new XeApiClient();
            var openExchangeApiClient = new OpenExchangeApiClient();

            // Fallback: When currencies cannot be retrieved from the primary source.
            var currencyApiClient = new CurrencyApiClientWithFallback(openExchangeApiClient, xeApiClient);

            var exchangeRateFetcher = new ExchangeRateFetcher(currencyApiClient, repository, broadcastBlock);
            var exchangeRatePrinter = new ExchangeRatePrinter(broadcastBlock, repository);

            // Run the ProducerService
            var producerTask = Task.Run(async () =>
            {
                while (true)
                {
                    await exchangeRateFetcher.FetchAndStoreDataAsync();
                    await Task.Delay(10000); 
                }
            });

            // Run the ConsumerService
            var consumerTask = Task.Run(() => exchangeRatePrinter.PrintRealTimeExchangeRates());

            // Run the print stored data task (every 1 minute)
            var printStoredTask = Task.Run(async () =>
            {
                while (true)
                {
                    exchangeRatePrinter.PrintStoredExchangeRates();
                    await Task.Delay(60000); // Print stored data every 1 minute
                }
            });

            // Wait for both tasks to finish (they will run indefinitely)
            await Task.WhenAll(producerTask, consumerTask, printStoredTask);
        }
    }
}
