using System.Collections.Generic;

namespace TradeCancelling
{
    public class AggregatedTradeMessageRecords
    {
        public string CompanyName { get; set; }

        public Queue<SimplifiedTradeMessageRecords> TimeSortedTradeMessages { get; set; } = new Queue<SimplifiedTradeMessageRecords>();
    }
}
