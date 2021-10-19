using System;
using System.Linq;

namespace TradeCancelling
{
    public class TradeMessageRecord
    {
        public DateTime MessageTime { get; set; }

        public String CompanyName { get; set; }

        public OrderTypesCollection.OrderTypes OrderType { get; set; }

        public Int32 Quantity { get; set; }

        public static bool TryParse(string stringValue, out TradeMessageRecord record)
        {
            record = null;
            if (string.IsNullOrWhiteSpace(stringValue)) return false;
            var recordParts = stringValue.Split(ExcessiveTradeCancellingConfiguration.ColumnSeparator);
            if (recordParts.Count() == 4)
            {
                if (String.IsNullOrWhiteSpace(recordParts[0])) return false;
                if (DateTime.TryParse(recordParts[0], System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime messageTime) == false) return false;

                if (String.IsNullOrWhiteSpace(recordParts[1])) return false;

                if (!(recordParts[2] == OrderTypesCollection.Instance.NewOrder.Code || recordParts[2] == OrderTypesCollection.Instance.Cancel.Code)) return false;

                if (Int32.TryParse(recordParts[3], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int quantity) == false) return false;

                record = new TradeMessageRecord();
                record.MessageTime = messageTime;
                record.CompanyName = recordParts[1];
                record.OrderType = OrderTypesCollection.Instance.Parse(recordParts[2]);

                record.Quantity = quantity;

                return true;
            }
            return false;
        }
    }
}
