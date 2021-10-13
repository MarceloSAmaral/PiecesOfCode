using System.Collections.Generic;
using System;
using System.Linq;

namespace CodeScreen.Assessments.TradeCancelling
{
    public static class ExcessiveTradeCancellingChecker
    {
        public static List<string> CompaniesInvolvedInExcessiveCancellations()
        {
            // Returns the list of companies that are involved in excessive cancelling.
            //TODO Implement
            List<string> excessiveCancellationsCompanies = new List<string>();
            var aggregatedtradeMessages = LoadSplittedRecords(ExcessiveTradeCancellingConfiguration.DatafileFullName);

            foreach (var companyRecord in aggregatedtradeMessages)
            {
                if (CheckHasExcessiveCancellations(companyRecord.TradeMessages) == true)
                {
                    excessiveCancellationsCompanies.Add(companyRecord.CompanyName);
                }
            }

            return excessiveCancellationsCompanies;
        }

        public static int TotalNumberOfWellBehavedCompanies()
        {
            // Returns the total number of companies that are not involved in any excessive cancelling.
            //TODO Implement
            int totalNumberOfWellBehavedCompanies = 0;
            var aggregatedtradeMessages = LoadSplittedRecords(ExcessiveTradeCancellingConfiguration.DatafileFullName);

            foreach (var companyRecord in aggregatedtradeMessages)
            {
                if (CheckHasExcessiveCancellations(companyRecord.TradeMessages) == false)
                {
                    totalNumberOfWellBehavedCompanies++;
                }
            }

            return totalNumberOfWellBehavedCompanies;
        }

        private static IEnumerable<TradeMessageRecord> GetRecordsFromFile(string fileRelativePath)
        {
            using var file = new System.IO.StreamReader(fileRelativePath);
            string lineContent;
            while ((lineContent = file.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(lineContent))
                {
                    var recordParts = lineContent.Split(ExcessiveTradeCancellingConfiguration.ColumnSeparator);
                    if (recordParts.Count() == 4)
                    {
                        if (String.IsNullOrWhiteSpace(recordParts[0])) continue;
                        if (DateTime.TryParse(recordParts[0], System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime messageTime) == false) continue;

                        if (String.IsNullOrWhiteSpace(recordParts[1])) continue;

                        if (!(recordParts[2] == "D" || recordParts[2] == "F")) continue;

                        if (Int32.TryParse(recordParts[3], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int quantity) == false) continue;

                        TradeMessageRecord record = new TradeMessageRecord();
                        record.MessageTime = messageTime;
                        record.CompanyName = recordParts[1];
                        record.OrderType = recordParts[2];

                        record.Quantity = quantity;

                        yield return record;
                    }
                }
            }
            file.Close();
        }

        private static IEnumerable<AggregatedTradeMessaRecords> LoadSplittedRecords(string fileRelativePath)
        {
            Dictionary<string, AggregatedTradeMessaRecords> aggregatedRecords = new Dictionary<string, AggregatedTradeMessaRecords>();
            foreach (var tradeMessage in GetRecordsFromFile(fileRelativePath))
            {
                if (aggregatedRecords.ContainsKey(tradeMessage.CompanyName))
                {
                    var companyRecords = aggregatedRecords[tradeMessage.CompanyName];
                    companyRecords.TradeMessages.Add(new SimplifiedTradeMessageRecords() { MessageTime = tradeMessage.MessageTime, OrderType = tradeMessage.OrderType, Quantity = tradeMessage.Quantity });
                }
                else
                {
                    AggregatedTradeMessaRecords newCompanyRecord = new AggregatedTradeMessaRecords();
                    newCompanyRecord.CompanyName = tradeMessage.CompanyName;
                    newCompanyRecord.TradeMessages.Add(new SimplifiedTradeMessageRecords() { MessageTime = tradeMessage.MessageTime, OrderType = tradeMessage.OrderType, Quantity = tradeMessage.Quantity });
                    aggregatedRecords.Add(tradeMessage.CompanyName, newCompanyRecord);
                }
            }

            foreach (var item in aggregatedRecords)
            {
                yield return item.Value;
            }
        }

        private static bool CheckHasExcessiveCancellations(List<SimplifiedTradeMessageRecords> tradeMessages)
        {
            if (tradeMessages.Count == 1)
            {
                if (tradeMessages[0].OrderType == "F") return true;
            }
            else
            {
                int maxTradeMessagesIndex = tradeMessages.Count - 1;
                DateTime lastDatetimeLimit = tradeMessages[0].MessageTime.AddSeconds(ExcessiveTradeCancellingConfiguration.TimeWindowInSeconds);
                for (int i = 0; i <= maxTradeMessagesIndex; i++)
                {
                    (int totalOrders, int totalCancellations) = ComputeCancellations(tradeMessages, tradeMessages[i].MessageTime, tradeMessages[i].MessageTime.AddSeconds(ExcessiveTradeCancellingConfiguration.TimeWindowInSeconds));
                    if (i == maxTradeMessagesIndex)
                    {
                        if (tradeMessages[i].MessageTime > lastDatetimeLimit)
                        {
                            if (totalCancellations >= totalOrders / 3)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (totalCancellations >= totalOrders / 3)
                        {
                            return true;
                        }
                    }
                    lastDatetimeLimit = tradeMessages[i].MessageTime.AddSeconds(ExcessiveTradeCancellingConfiguration.TimeWindowInSeconds);
                }
            }
            return false;
        }

        private static (int TotalOrders, int TotalCancellations) ComputeCancellations(List<SimplifiedTradeMessageRecords> tradeMessages, DateTime startDatetime, DateTime datetimeLimit)
        {
            int totalOrders = 0;
            int totalCancellations = 0;
            int maxTradeMessagesIndex = tradeMessages.Count - 1;
            for (int i = 0; i <= maxTradeMessagesIndex; i++)
            {
                if (tradeMessages[i].MessageTime < startDatetime) continue;
                if (tradeMessages[i].MessageTime <= datetimeLimit)
                {
                    totalOrders = totalOrders + tradeMessages[i].Quantity;
                    if (tradeMessages[i].OrderType == "F")
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
