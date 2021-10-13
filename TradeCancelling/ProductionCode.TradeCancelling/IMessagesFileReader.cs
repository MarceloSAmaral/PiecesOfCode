using System.Collections.Generic;

namespace ProductionCode.TradeCancelling
{
    public interface IMessagesFileReader
    {
        IAsyncEnumerable<TradeMessageRecord> GetRecordsFromFile(string fileRelativePath);
        IAsyncEnumerable<AggregatedTradeMessageRecords> LoadSplittedRecords(string fileRelativePath);
    }
}