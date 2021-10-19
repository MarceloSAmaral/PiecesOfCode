using System;

namespace TradeCancelling
{
    public class SimplifiedTradeMessageRecords
    {
        public DateTime MessageTime { get; set; }

        public OrderTypesCollection.OrderTypes OrderType { get; set; }

        public Int32 Quantity { get; set; }
    }
}
