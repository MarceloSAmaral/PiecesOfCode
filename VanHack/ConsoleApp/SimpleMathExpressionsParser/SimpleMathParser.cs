using System;

namespace ConsoleApp.SimpleMathExpressionsParser
{
    public static class SimpleMathParserChallenge
    {
        public static double Calc(string mathExpression)
        {
            SimpleMathematicalExpression expressionElement = SimpleMathExpressionParser.Parse(mathExpression);
            double result = expressionElement.Calc();
            Console.WriteLine($"Result: {result}");
            return result;
        }
    }
}
