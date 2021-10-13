using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class CodeV02
    {
        public static async Task RunTest()
        {
            Console.WriteLine("Code Test V02");
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            List<string> companiesInvolvedInExcessiveCancellationsResult;
            int totalNumberOfWellBehavedCompaniesResult;
            ProductionCode.TradeCancelling.ExcessiveTradeCancellingConfiguration.DatafileFullName = "../../../../Trades.data";
            companiesInvolvedInExcessiveCancellationsResult = await ProductionCode.TradeCancelling.ExcessiveTradeCancellingChecker.GetCompaniesInvolvedInExcessiveCancellations();
            totalNumberOfWellBehavedCompaniesResult = await ProductionCode.TradeCancelling.ExcessiveTradeCancellingChecker.GetTotalNumberOfWellBehavedCompanies();
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
