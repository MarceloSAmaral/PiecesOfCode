﻿using System.Collections.Generic;
using Xunit;

namespace TradeCancelling.Tests
{
    public class ExcessiveTradeCancellingCheckerTest
    {
        [Fact]
        public void TestCompaniesInvolvedInExcessiveCancellations()
        {
            ExcessiveTradeCancellingConfiguration.DatafileFullName = "Trades.data";
            List<string> expectedCompaniesInvolvedInExcessiveCancellations = new List<string>();
            expectedCompaniesInvolvedInExcessiveCancellations.Add("Ape accountants");
            expectedCompaniesInvolvedInExcessiveCancellations.Add("Cauldron cooking");
            var result = ExcessiveTradeCancellingChecker.GetCompaniesInvolvedInExcessiveCancellations().Result;
            Assert.Equal(expectedCompaniesInvolvedInExcessiveCancellations, result);
        }
    }
}
