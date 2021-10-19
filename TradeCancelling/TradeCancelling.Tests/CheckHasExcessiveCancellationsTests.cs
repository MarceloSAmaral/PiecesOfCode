using System;
using System.Collections.Generic;
using Xunit;

namespace TradeCancelling.Tests
{
    public class CheckHasExcessiveCancellationsTests
    {
        [Theory(DisplayName = "Checker must accept valid scenarios")]
        [MemberData(nameof(CasesOfValidTradeMessages))]
        public void TestingCasesOfValidTradeMessages(SimplifiedTradeMessageRecords[] tradeMessages)
        {
            Xunit.Assert.False(ExcessiveTradeCancellingChecker.CheckHasExcessiveCancellations(tradeMessages));
        }

        public static IEnumerable<Object[]> CasesOfValidTradeMessages()
        {
            yield return OnlyOneTradeMessageOfNewOrder();
            yield return CancelOrdersAreLessThan30PercentAtSameTimestamp();
            yield return CancelOrdersAreLessThan30PercentInTimeWindow();
        }

        private static object[] OnlyOneTradeMessageOfNewOrder()
        {
            List<SimplifiedTradeMessageRecords> tradeMessages = new List<SimplifiedTradeMessageRecords>();
            tradeMessages.Add(new SimplifiedTradeMessageRecords()
            {
                MessageTime = new DateTime(2021, 1, 1, 1, 1, 1),
                OrderType = OrderTypesCollection.Instance.NewOrder,
                Quantity = 1
            });

            return new object[] { tradeMessages.ToArray() };
        }

        private static object[] CancelOrdersAreLessThan30PercentAtSameTimestamp()
        {
            List<SimplifiedTradeMessageRecords> tradeMessages = new List<SimplifiedTradeMessageRecords>();
            tradeMessages.Add(new SimplifiedTradeMessageRecords()
            {
                MessageTime = new DateTime(2021, 1, 1, 1, 1, 1),
                OrderType = OrderTypesCollection.Instance.NewOrder,
                Quantity = 66
            });

            tradeMessages.Add(new SimplifiedTradeMessageRecords()
            {
                MessageTime = new DateTime(2021, 1, 1, 1, 1, 1),
                OrderType = OrderTypesCollection.Instance.Cancel,
                Quantity = 32
            });

            return new object[] { tradeMessages.ToArray() };
        }

        private static object[] CancelOrdersAreLessThan30PercentInTimeWindow()
        {
            List<SimplifiedTradeMessageRecords> tradeMessages = new List<SimplifiedTradeMessageRecords>();
            tradeMessages.Add(new SimplifiedTradeMessageRecords()
            {
                MessageTime = new DateTime(2021, 1, 1, 1, 1, 1),
                OrderType = OrderTypesCollection.Instance.NewOrder,
                Quantity = 32
            });

            tradeMessages.Add(new SimplifiedTradeMessageRecords()
            {
                MessageTime = new DateTime(2021, 1, 1, 1, 1, 30),
                OrderType = OrderTypesCollection.Instance.NewOrder,
                Quantity = 66
            });

            return new object[] { tradeMessages.ToArray() };
        }

        [Theory(DisplayName = "Checker must reject invalid scenarios")]
        [MemberData(nameof(CasesOfInvalidTradeMessages))]
        public void TestingCasesOfInvalidTradeMessages(SimplifiedTradeMessageRecords[] tradeMessages)
        {
            Xunit.Assert.True(ExcessiveTradeCancellingChecker.CheckHasExcessiveCancellations(tradeMessages));
        }

        public static IEnumerable<Object[]> CasesOfInvalidTradeMessages()
        {
            yield return OnlyOneTradeMessageOfCancellation();
            yield return CancelOrdersAreMoreOrEqualsThan30PercentAtSameTimestamp();
            yield return CancelOrdersAreMoreOrEqualsThan30PercentInTimeWindow();
            yield return ExcessiveCancelOrdersAfterAValidTimeWindow();
        }

        private static object[] OnlyOneTradeMessageOfCancellation()
        {
            List<SimplifiedTradeMessageRecords> tradeMessages = new List<SimplifiedTradeMessageRecords>();
            tradeMessages.Add(new SimplifiedTradeMessageRecords()
            {
                MessageTime = new DateTime(2021, 1, 1, 1, 1, 1),
                OrderType = OrderTypesCollection.Instance.Cancel,
                Quantity = 1
            });

            return new object[] { tradeMessages.ToArray() };
        }

        private static object[] CancelOrdersAreMoreOrEqualsThan30PercentAtSameTimestamp()
        {
            List<SimplifiedTradeMessageRecords> tradeMessages = new List<SimplifiedTradeMessageRecords>();
            tradeMessages.Add(new SimplifiedTradeMessageRecords()
            {
                MessageTime = new DateTime(2021, 1, 1, 1, 1, 1),
                OrderType = OrderTypesCollection.Instance.NewOrder,
                Quantity = 66
            });

            tradeMessages.Add(new SimplifiedTradeMessageRecords()
            {
                MessageTime = new DateTime(2021, 1, 1, 1, 1, 1),
                OrderType = OrderTypesCollection.Instance.Cancel,
                Quantity = 33
            });

            return new object[] { tradeMessages.ToArray() };
        }

        private static object[] CancelOrdersAreMoreOrEqualsThan30PercentInTimeWindow()
        {
            List<SimplifiedTradeMessageRecords> tradeMessages = new List<SimplifiedTradeMessageRecords>();
            tradeMessages.Add(new SimplifiedTradeMessageRecords()
            {
                MessageTime = new DateTime(2021, 1, 1, 1, 1, 1),
                OrderType = OrderTypesCollection.Instance.NewOrder,
                Quantity = 66
            });

            tradeMessages.Add(new SimplifiedTradeMessageRecords()
            {
                MessageTime = new DateTime(2021, 1, 1, 1, 1, 30),
                OrderType = OrderTypesCollection.Instance.Cancel,
                Quantity = 33
            });

            return new object[] { tradeMessages.ToArray() };
        }


        private static object[] ExcessiveCancelOrdersAfterAValidTimeWindow()
        {
            List<SimplifiedTradeMessageRecords> tradeMessages = new List<SimplifiedTradeMessageRecords>();
            tradeMessages.Add(new SimplifiedTradeMessageRecords()
            {
                MessageTime = new DateTime(2021, 1, 1, 1, 1, 1),
                OrderType = OrderTypesCollection.Instance.NewOrder,
                Quantity = 100
            });

            tradeMessages.Add(new SimplifiedTradeMessageRecords()
            {
                MessageTime = new DateTime(2021, 1, 1, 1, 1, 1),
                OrderType = OrderTypesCollection.Instance.Cancel,
                Quantity = 1
            });

            tradeMessages.Add(new SimplifiedTradeMessageRecords()
            {
                MessageTime = new DateTime(2021, 1, 1, 1, 1, 40),
                OrderType = OrderTypesCollection.Instance.Cancel,
                Quantity = 1
            });

            return new object[] { tradeMessages.ToArray() };
        }

    }
}
