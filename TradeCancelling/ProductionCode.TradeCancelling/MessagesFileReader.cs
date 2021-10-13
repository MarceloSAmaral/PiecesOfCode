using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductionCode.TradeCancelling
{
    public class MessagesFileReader : IMessagesFileReader
    {
        public async IAsyncEnumerable<TradeMessageRecord> GetRecordsFromFile(string fileRelativePath)
        {
            using var file = new System.IO.StreamReader(fileRelativePath);
            string lineContent;
            while ((lineContent = await file.ReadLineAsync()) != null)
            {
                if (!string.IsNullOrWhiteSpace(lineContent))
                {
                    if ((TradeMessageRecord.TryParse(lineContent, out TradeMessageRecord record)) == true)
                    {
                        yield return record;
                    }
                }
            }
            file.Close();
        }

        public async IAsyncEnumerable<AggregatedTradeMessageRecords> LoadSplittedRecords(string fileRelativePath)
        {
            Dictionary<string, AggregatedTradeMessageRecords> aggregatedRecords = new Dictionary<string, AggregatedTradeMessageRecords>();
            await foreach (var tradeMessage in GetRecordsFromFile(fileRelativePath))
            {
                if (aggregatedRecords.ContainsKey(tradeMessage.CompanyName))
                {
                    var companyRecords = aggregatedRecords[tradeMessage.CompanyName];
                    companyRecords.TimeSortedTradeMessages.Enqueue(new SimplifiedTradeMessageRecords() { MessageTime = tradeMessage.MessageTime, OrderType = tradeMessage.OrderType, Quantity = tradeMessage.Quantity });
                }
                else
                {
                    AggregatedTradeMessageRecords newCompanyRecord = new AggregatedTradeMessageRecords();
                    newCompanyRecord.CompanyName = tradeMessage.CompanyName;
                    newCompanyRecord.TimeSortedTradeMessages.Enqueue(new SimplifiedTradeMessageRecords() { MessageTime = tradeMessage.MessageTime, OrderType = tradeMessage.OrderType, Quantity = tradeMessage.Quantity });
                    aggregatedRecords.Add(tradeMessage.CompanyName, newCompanyRecord);
                }
            }

            foreach (var item in aggregatedRecords)
            {
                yield return item.Value;
            }
        }
    }
}
