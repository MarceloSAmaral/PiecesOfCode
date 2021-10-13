using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class ProductionCode
    {
        public static async Task RunSample()
        {
            Console.WriteLine("Running production level code sample...");
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            List<string> companiesInvolvedInExcessiveCancellationsResult;
            int totalNumberOfWellBehavedCompaniesResult;
            global::ProductionCode.TradeCancelling.ExcessiveTradeCancellingConfiguration.DatafileFullName = "../../../../Trades.data";
            companiesInvolvedInExcessiveCancellationsResult = await global::ProductionCode.TradeCancelling.ExcessiveTradeCancellingChecker.GetCompaniesInvolvedInExcessiveCancellations();
            totalNumberOfWellBehavedCompaniesResult = await global::ProductionCode.TradeCancelling.ExcessiveTradeCancellingChecker.GetTotalNumberOfWellBehavedCompanies();
            stopwatch.Stop();
            Console.WriteLine($"Time to perform task: {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine("Companies that exceeded cancellations:");
            foreach (var item in companiesInvolvedInExcessiveCancellationsResult)
            {
                Console.WriteLine($"\t{item}");
            }
            Console.WriteLine($"Total Number Of Well Behaved Companies: {totalNumberOfWellBehavedCompaniesResult}");
        }
    }
}
