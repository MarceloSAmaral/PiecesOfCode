using System.Collections.Generic;
using Xunit;

namespace ProductionCode.TradeCancelling.Tests
{
    public class ExcessiveTradeCancellingCheckerTest
    {
        [Fact]
        public void TestCompaniesInvolvedInExcessiveCancellations()
        {
            List<string> expectedCompaniesInvolvedInExcessiveCancellations = new List<string>();
            expectedCompaniesInvolvedInExcessiveCancellations.Add("Ape accountants");
            expectedCompaniesInvolvedInExcessiveCancellations.Add("Cauldron cooking");
            var result = ExcessiveTradeCancellingChecker.GetCompaniesInvolvedInExcessiveCancellations().Result;
            Assert.Equal(expectedCompaniesInvolvedInExcessiveCancellations, result);
        }
    }
}
