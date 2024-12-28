using SharedUtils.ModelEntities;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace SharedUtils.DataFlow
{
    public class DataflowSetup
    {
        public BroadcastBlock<List<CurrencyPair>> CreateBroadcastBlock()
        {
            return new BroadcastBlock<List<CurrencyPair>>(data => data);
        }
    }
}
