using System;

namespace SharedUtils.ModelEntities
{
    public class CurrencyPair
    {
        public string PairName { get; set; }
        public decimal Value { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
