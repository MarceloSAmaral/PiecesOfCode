using ConsoleApp.RomanNumeralNotation;
using ConsoleApp.SimpleMathExpressionsParser;
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            string roman = RomanNumbers.WriteInRomanNotation(984);
            decimal decima = RomanNumbers.WriteDecimalFromRoman("MMVIII");
            List<double> mathResults = new List<double>();
            mathResults.Add(SimpleMathParserChallenge.Calc("2 / (2 + 3) * 4.33"));
            mathResults.Add(SimpleMathParserChallenge.Calc("2 / (2 + 3) * 4.33 - -6"));
        }
    }
}
