using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace ProductionCode.TradeCancelling
{
    public static class ExcessiveTradeCancellingChecker
    {
        public static async Task<List<string>> GetCompaniesInvolvedInExcessiveCancellations()
        {
            // Returns the list of companies that are involved in excessive cancelling.
            //TODO Implement
            ConcurrentQueue<string> excessiveCancellationsCompanies = new ConcurrentQueue<string>();
            List<Thread> threads = new List<Thread>();
            IMessagesFileReader messagesReader = new MessagesFileReader();
            var aggregatedtradeMessages = messagesReader.LoadSplittedRecords(ExcessiveTradeCancellingConfiguration.DatafileFullName);

            await foreach (var companyRecord in aggregatedtradeMessages)
            {
                Thread processingCompanyThread = CreateThreadForChecker(companyRecord, excessiveCancellationsCompanies);
                threads.Add(processingCompanyThread);
            }

            WaitForCheckers(threads);

            return excessiveCancellationsCompanies.ToList();
        }

        public static async Task<int> GetTotalNumberOfWellBehavedCompanies()
        {
            // Returns the total number of companies that are not involved in any excessive cancelling.
            //TODO Implement
            Queue<String> allCompanies = new Queue<string>();
            ConcurrentQueue<string> excessiveCancellationsCompanies = new ConcurrentQueue<string>();
            List<Thread> threads = new List<Thread>();
            IMessagesFileReader messagesReader = new MessagesFileReader();
            var aggregatedtradeMessages = messagesReader.LoadSplittedRecords(ExcessiveTradeCancellingConfiguration.DatafileFullName);

            await foreach (var companyRecord in aggregatedtradeMessages)
            {
                allCompanies.Enqueue(companyRecord.CompanyName);
                Thread processingCompanyThread = CreateThreadForChecker(companyRecord, excessiveCancellationsCompanies);
                threads.Add(processingCompanyThread);
            }

            WaitForCheckers(threads);

            return allCompanies.Count - excessiveCancellationsCompanies.Count;
        }

        private static Thread CreateThreadForChecker(AggregatedTradeMessageRecords companyRecord, ConcurrentQueue<string> excessiveCancellationsCompanies)
        {

            Thread processingCompanyThread = new Thread(new ParameterizedThreadStart(x => CheckHasExcessiveCancellations( (x as object[])[0] as AggregatedTradeMessageRecords, (x as object[])[1] as ConcurrentQueue<string>)));
            processingCompanyThread.Name = $"Check HasExcessiveCancellations {companyRecord.CompanyName}";
            processingCompanyThread.IsBackground = false;
            processingCompanyThread.Priority = ThreadPriority.Normal;
            processingCompanyThread.Start(new object[] { companyRecord, excessiveCancellationsCompanies });
            return processingCompanyThread;
        }

        private static void WaitForCheckers(List<Thread> threads)
        {
            bool liveThreads = true;
            while (liveThreads)
            {
                liveThreads = false;
                for (int i = 0; i < threads.Count - 1; i++)
                {
                    if (threads[i].IsAlive)
                    {
                        liveThreads = true;
                        threads[i].Join();
                    }
                }
            }
        }

        private static void CheckHasExcessiveCancellations(AggregatedTradeMessageRecords companyRecord, ConcurrentQueue<string> outputQueue)
        {
            if (CheckHasExcessiveCancellations(companyRecord.TimeSortedTradeMessages.ToArray()))
            {
                outputQueue.Enqueue(companyRecord.CompanyName);
            }
        }

        public static bool CheckHasExcessiveCancellations(SimplifiedTradeMessageRecords[] tradeMessages)
        {
            if (HasOnlyOneTradeMessageAndItIsACancellationOne(tradeMessages)) return true;
            int startTimeWindowIndexOptmizer = 0;
            DateTime lastTimeWindowStart = new DateTime(1900, 1, 1);
            int maxTradeMessagesIndex = tradeMessages.Count() - 1;
            for (int i = 0; i <= maxTradeMessagesIndex; i++)
            {
                if (ShouldMoveStartTimeWindowIndexOptmizer(lastTimeWindowStart, tradeMessages[i].MessageTime))
                {
                    lastTimeWindowStart = tradeMessages[i].MessageTime;
                    startTimeWindowIndexOptmizer = i;
                }
                (int totalOrders, int totalCancellations) = ComputeTotalOrdesAndCancellationsInTimeWindow(tradeMessages, tradeMessages[i].MessageTime, tradeMessages[i].MessageTime.AddSeconds(ExcessiveTradeCancellingConfiguration.TimeWindowInSeconds), startTimeWindowIndexOptmizer);
                if (AreTotalCancellationsAboveLimit(totalOrders, totalCancellations, ExcessiveTradeCancellingConfiguration.MaxPercentageOfCancellations)) return true;
            }

            return false;
        }

        private static bool ShouldMoveStartTimeWindowIndexOptmizer(DateTime lastTimeWindowStart, DateTime messageTime)
        {
            if (messageTime > lastTimeWindowStart)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the total of cancellation orders exceeds the percentage upper limit in relation of total orders.
        /// </summary>
        /// <param name="totalOrders">total of orders.</param>
        /// <param name="totalCancellations">total of cancellation orders.</param>
        /// <param name="upperLimitPercentage">A percentage x => 0 < x < 100.</param>
        /// <returns></returns>
        private static bool AreTotalCancellationsAboveLimit(int totalOrders, int totalCancellations, double upperLimitPercentage)
        {
            if (upperLimitPercentage <= 0 || upperLimitPercentage >= 1) throw new ArgumentOutOfRangeException(nameof(upperLimitPercentage), $" {nameof(upperLimitPercentage)} must be 0 < X < 1.");
            if (totalCancellations >= totalOrders * upperLimitPercentage)
            {
                return true;
            }
            return false;
        }

        private static bool HasOnlyOneTradeMessageAndItIsACancellationOne(SimplifiedTradeMessageRecords[] tradeMessages)
        {
            if (tradeMessages.Count() == 1)
            {
                if (tradeMessages[0].OrderType == OrderTypesCollection.Instance.Cancel) return true;
            }
            return false;
        }

        private static (int TotalOrders, int TotalCancellations) ComputeTotalOrdesAndCancellationsInTimeWindow(SimplifiedTradeMessageRecords[] tradeMessages, DateTime startDatetime, DateTime datetimeLimit, int startTimeWindowIndexOptmizer = 0)
        {
            int totalOrders = 0;
            int totalCancellations = 0;
            int maxTradeMessagesIndex = tradeMessages.Count() - 1;
            for (int i = startTimeWindowIndexOptmizer; i <= maxTradeMessagesIndex; i++)
            {
                if (tradeMessages[i].MessageTime < startDatetime) continue;
                if (tradeMessages[i].MessageTime <= datetimeLimit)
                {
                    totalOrders = totalOrders + tradeMessages[i].Quantity;
                    if (tradeMessages[i].OrderType == OrderTypesCollection.Instance.Cancel)
                    {
                        totalCancellations = totalCancellations + tradeMessages[i].Quantity;
                    }
                }
                else
                {
                    break;
                }
            }
            return (totalOrders, totalCancellations);
        }

    }
}
