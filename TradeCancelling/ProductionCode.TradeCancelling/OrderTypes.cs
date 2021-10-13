using System;
using System.Collections.Generic;

namespace ProductionCode.TradeCancelling
{
    /// <summary>
    /// This Collection holds instace values of OrderTypes.
    /// </summary>
    /// <remarks>
    /// This approach both allows to use a typed value ir our code and also avoids a concurrency problem when
    /// we use the approch where the OrderTypes class holds static named instances.
    /// </remarks>
    public sealed class OrderTypesCollection
    {
        private static volatile OrderTypesCollection instance = null;

        public static OrderTypesCollection Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (typeof(OrderTypesCollection))
                    {
                        if (instance == null)
                        {
                            instance = new OrderTypesCollection();
                        }
                    }
                }
                return instance;
            }
        }


        public OrderTypes NewOrder = new OrderTypes("D");
        public OrderTypes Cancel = new OrderTypes("F");

        public OrderTypes Parse(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                throw new ArgumentException();
            }

            foreach (var item in OrderTypes.GetList())
            {
                if (item.Code == stringValue) return item;
            }
            throw new IndexOutOfRangeException();
        }

        public class OrderTypes
        {
            internal OrderTypes(string code)
            {
                Code = code;
                _listOfValues.Add(this);
            }
            public string Code { get; private set; }


            private static List<OrderTypes> _listOfValues = new List<OrderTypes>();
            internal static List<OrderTypes> GetList() { return _listOfValues; }
        }
    }
}
