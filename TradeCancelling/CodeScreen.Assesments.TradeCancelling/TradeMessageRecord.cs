using System;

namespace CodeScreen.Assessments.TradeCancelling
{
    public class TradeMessageRecord
    {
        public DateTime MessageTime { get; set; }

        public String CompanyName { get; set; }

        public string OrderType { get; set; }

        public Int32 Quantity { get; set; }
    }
}
