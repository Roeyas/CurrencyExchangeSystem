using SharedUtils.ModelEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks.Dataflow;

namespace ConsumerService.Services
{
    public class ExchangeRatePrinter
    {
        private readonly BroadcastBlock<List<CurrencyPair>> _broadcastBlock;
        private readonly Repository _repository;

        public ExchangeRatePrinter(BroadcastBlock<List<CurrencyPair>> broadcastBlock, Repository repository)
        {
            _broadcastBlock = broadcastBlock;
            _repository = repository;
        }

        public void PrintRealTimeExchangeRates()
        {
            _broadcastBlock.LinkTo(new ActionBlock<List<CurrencyPair>>(currencyPairs =>
            {
                Console.WriteLine("\nReal-time Exchange Rates:");
                foreach (var pair in currencyPairs)
                {
                    Console.WriteLine($"{pair.PairName}: {pair.Value} (Updated: {pair.LastModifiedDate})");
                }
            }));
        }
        public void PrintStoredExchangeRates()
        {
            var currencyPairs = _repository.GetCurrencyRatesFromDB();

            Console.WriteLine("\nStored Exchange Rates:");

            if (currencyPairs.Count == 0)
                Console.WriteLine("No records at the moment");
            else
            {
                foreach (var pair in currencyPairs)
                {
                    Console.WriteLine($"{pair.PairName}: {pair.Value} (Updated: {pair.LastModifiedDate})");
                }
            }
        }
    }
}
