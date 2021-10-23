using ConsoleApp.SimpleMathExpressionsParser;
using System;
using System.Collections.Generic;
using Xunit;

namespace CodeChallengeTests
{
    public class SimpleMathExpressionsTests
    {
        [Theory(DisplayName ="Testing simple math expressions")]
        [MemberData(nameof(CasesOfMathExpressions))]
        public void TestingMathExpressions(string mathExpression, double expectedResult)
        {
            SimpleMathematicalExpression mathExpressionHandler = SimpleMathExpressionParser.Parse(mathExpression);
            Assert.Equal(mathExpressionHandler.Calc(), expectedResult, 5);
        }

        public static IEnumerable<Object[]> CasesOfMathExpressions()
        {
            yield return SimplePositiveLiteralValue();
            yield return SimpleNegativeLiteralValue();
            yield return SimplePositiveExpression();
            yield return SimpleNegativeExpression();
            yield return ImplicityNegativeMultiplierOverExpressionCaseA();
            yield return ImplicityNegativeMultiplierOverExpressionCaseB();
            yield return ImplicityNegativeMultiplierOverExpressionCaseC();
            yield return NoSpaceBetweenLiteralsAndOperator();
            yield return SpaceBetweenLiteralsAndOperator();
            yield return NoSpaceBetweenRightLiteralAndOperator();
            yield return NoSpaceBetweenLeftLiteralAndOperator();
            yield return SimpleMultiplication();
            yield return SimpleDivision();
            yield return SimpleSubtraction();
            foreach (var item in OtheCases())
            {
                yield return item;
            }
        }

        private static object[]   SimplePositiveLiteralValue()
        {
            return new object[] { "123", 123 };
        }

        private static object[] SimpleNegativeLiteralValue()
        {
            return new object[] { "-123", -123 };
        }

        private static object[] SimplePositiveExpression()
        {
            return new object[] { "(123)", 123 };
        }

        private static object[] SimpleNegativeExpression()
        {
            return new object[] { "(-123)", -123 };
        }

        private static object[] ImplicityNegativeMultiplierOverExpressionCaseA()
        {
            return new object[] { "-(123)", -123 };
        }

        private static object[] ImplicityNegativeMultiplierOverExpressionCaseB()
        {
            return new object[] { "-(-123)", 123 };
        }

        private static object[] ImplicityNegativeMultiplierOverExpressionCaseC()
        {
            return new object[] { "-(1 + 2)", -3 };
        }

        private static object[] NoSpaceBetweenLiteralsAndOperator()
        {
            return new object[] { "1+2", 3 };
        }

        private static object[] SpaceBetweenLiteralsAndOperator()
        {
            return new object[] { "1 + 2", 3 };
        }

        private static object[] NoSpaceBetweenRightLiteralAndOperator()
        {
            return new object[] { "1 +2", 3 };
        }

        private static object[] NoSpaceBetweenLeftLiteralAndOperator()
        {
            return new object[] { "1+ 2", 3};
        }

        private static object[] SimpleMultiplication()
        {
            return new object[] { "2 * 3", 6 };
        }
        private static object[] SimpleDivision()
        {
            return new object[] { "9 / 3", 3 };
        }
        private static object[] SimpleSubtraction()
        {
            return new object[] { "3 - 2", 1 };
        }




        private static IEnumerable<Object[]> OtheCases()
        {
            yield return new object[] {"6 + -( -4)", 10 };

            yield return new object[] { "6 + -4", 2 };

            yield return new object[] { "2+3*4", 14 };

            yield return new object[] { "2*3+4", 10 };

            yield return new object[] { "2+3*4+5", 19 };

            yield return new object[] { "(2+3)", 5 };

            yield return new object[] { "2 / (2 + 3) * 4.33", 1.732 };

            yield return new object[] { "2 / (2 + 3) * 4.33 - -6", 7.732 };

            yield return new object[] { "6 + -( -4)", 10 };

            yield return new object[] { "2+(3*4)+5", 19 };

            yield return new object[] { "2*3*4", 24 };
        }
    }
}
