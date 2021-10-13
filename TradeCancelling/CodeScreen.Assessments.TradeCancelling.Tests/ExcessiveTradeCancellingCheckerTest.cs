using NUnit.Framework;
using System.Collections.Generic;

namespace CodeScreen.Assessments.TradeCancelling.Tests
{
    [TestFixture]
    public class ExcessiveTradeCancellingCheckerTest
    {
        [TestCase]
        public void TestCompaniesInvolvedInExcessiveCancellations()
        {
            List<string> expectedCompaniesInvolvedInExcessiveCancellations = new List<string>();
            expectedCompaniesInvolvedInExcessiveCancellations.Add("Ape accountants");
            expectedCompaniesInvolvedInExcessiveCancellations.Add("Cauldron cooking");

            Assert.AreEqual(expectedCompaniesInvolvedInExcessiveCancellations, ExcessiveTradeCancellingChecker.CompaniesInvolvedInExcessiveCancellations());
        }

    }
}
