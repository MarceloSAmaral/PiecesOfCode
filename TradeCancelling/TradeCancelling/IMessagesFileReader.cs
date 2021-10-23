using System.Collections.Generic;

namespace TradeCancelling
{
    public interface IMessagesFileReader
    {
        IAsyncEnumerable<TradeMessageRecord> GetRecordsFromFile(string fileRelativePath);
        IAsyncEnumerable<CompanyAggregatedTradeMessageRecords> GetCompanyAggregatedRecords(string fileRelativePath);
    }
}