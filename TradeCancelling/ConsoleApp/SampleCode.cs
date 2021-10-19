using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class SampleCode
    {
        public static async Task RunSample()
        {
            Console.WriteLine("Running code sample...");
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            List<string> companiesInvolvedInExcessiveCancellationsResult;
            int totalNumberOfWellBehavedCompaniesResult;
            TradeCancelling.ExcessiveTradeCancellingConfiguration.DatafileFullName = "../../../../Trades.data";
            companiesInvolvedInExcessiveCancellationsResult = await TradeCancelling.ExcessiveTradeCancellingChecker.GetCompaniesInvolvedInExcessiveCancellations();
            totalNumberOfWellBehavedCompaniesResult = await TradeCancelling.ExcessiveTradeCancellingChecker.GetTotalNumberOfWellBehavedCompanies();
            stopwatch.Stop();
            Console.WriteLine($"Time to perform task: {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine("Companies that exceeded cancellations:");
            foreach (var item in companiesInvolvedInExcessiveCancellationsResult)
            {
                Console.WriteLine($"\t{item}");
            }
            Console.WriteLine($"Total number of well behaved companies: {totalNumberOfWellBehavedCompaniesResult}");
        }
    }
}
