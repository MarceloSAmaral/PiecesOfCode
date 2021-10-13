using CodeScreen.Assessments.TradeCancelling;
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    class CodeV01
    {
        public static void RunTest()
        {
            Console.WriteLine("Code Test V01");
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            List<string> companiesInvolvedInExcessiveCancellationsResult;
            int TotalNumberOfWellBehavedCompaniesResult;
            ExcessiveTradeCancellingConfiguration.DatafileFullName = "../../../../Trades.data";
            companiesInvolvedInExcessiveCancellationsResult = ExcessiveTradeCancellingChecker.CompaniesInvolvedInExcessiveCancellations();
            TotalNumberOfWellBehavedCompaniesResult = ExcessiveTradeCancellingChecker.TotalNumberOfWellBehavedCompanies();
            stopwatch.Stop();
            Console.WriteLine($"Time to perform task: {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine("Companies that exceeded cancellations:");
            foreach (var item in companiesInvolvedInExcessiveCancellationsResult)
            {
                Console.WriteLine($"\t{item}");
            }
            Console.WriteLine($"Total Number Of Well Behaved Companies: {TotalNumberOfWellBehavedCompaniesResult}");
        }
    }
}
