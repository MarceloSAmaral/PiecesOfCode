using System.Collections.Generic;

namespace CodeScreen.Assessments.TradeCancelling
{
    public class AggregatedTradeMessaRecords
    {
        public string CompanyName { get; set; }

        public List<SimplifiedTradeMessageRecords> TradeMessages { get; set; } = new List<SimplifiedTradeMessageRecords>();
    }
}
