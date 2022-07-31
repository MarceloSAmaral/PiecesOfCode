namespace Challenges.Test
{
    public class BalancedDelimitersCheckerTests
    {
        [Fact]
        public void BalanceTextShouldBeApproved()
        {
            string balancedText = "[()]{}{[()()]()}";
            BalancedDelimitersChecker.CheckForBalancedDelimiters(balancedText);
            Assert.True(true);//Just works!
        }

        [Fact]
        public void UnbalanceTextShouldBeApproved()
        {
            string unbalancedText = "[(])";
            Assert.Throws<WrongClosingDelimiterCharacterException>(() => BalancedDelimitersChecker.CheckForBalancedDelimiters(unbalancedText));
        }

    }
}
