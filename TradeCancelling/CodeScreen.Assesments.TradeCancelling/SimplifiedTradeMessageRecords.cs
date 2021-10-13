using System;

namespace CodeScreen.Assessments.TradeCancelling
{
    public class SimplifiedTradeMessageRecords
    {
        public DateTime MessageTime { get; set; }

        public string OrderType { get; set; }

        public Int32 Quantity { get; set; }
    }
}
