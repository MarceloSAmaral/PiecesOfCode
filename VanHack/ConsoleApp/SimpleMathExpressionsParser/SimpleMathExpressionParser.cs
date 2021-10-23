using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.SimpleMathExpressionsParser
{
    public static class SimpleMathExpressionParser
    {
        public static SimpleMathematicalExpression Parse(string mathExpression)
        {
            var textualMathExpression = mathExpression;

            List<MathExpressionElement> listofElements;
            listofElements = DecomposeExpressionInElements(mathExpression);
            listofElements = AdjustImplicityNegativeMultiplierOperator(listofElements);
            var (leftExpressions, thisNodeOperator, rightExpressions) = Split(listofElements);

            Console.WriteLine($"Expression: {mathExpression}");
            Console.WriteLine($"Left: \"{JoinExpressions(leftExpressions)}\"    Operator: \"{thisNodeOperator}\"    Right: \"{JoinExpressions(rightExpressions)}\"");

            SimpleMathematicalExpression mathematicalExpressionObj = new SimpleMathematicalExpression();

            if ((leftExpressions.Count == 1) && (leftExpressions[0].TypeOfElement == TypesOfElements.LiteralValue))
            {
                mathematicalExpressionObj._LeftNodeLiteralValue = double.Parse(leftExpressions[0].TextualValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                mathematicalExpressionObj.LeftNodeExpression = SimpleMathExpressionParser.Parse(JoinExpressions(leftExpressions));
            }

            if ((rightExpressions.Count == 1) && (rightExpressions[0].TypeOfElement == TypesOfElements.LiteralValue))
            {
                mathematicalExpressionObj._RightNodeLiteralValue = double.Parse(rightExpressions[0].TextualValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                mathematicalExpressionObj.RightNodeExpression = SimpleMathExpressionParser.Parse(JoinExpressions(rightExpressions));
            }

            mathematicalExpressionObj.MathOperator = thisNodeOperator;
            return mathematicalExpressionObj;
        }

        private static object _SyncRoot = new object();
        private static System.Text.RegularExpressions.Regex _regex;
        private static System.Text.RegularExpressions.Regex GetParser()
        {
            if (_regex == null)
            {
                lock(_SyncRoot)
                {
                    if(_regex == null)
                    {
                        _regex = new System.Text.RegularExpressions.Regex(ConsoleApp.Properties.Resources.MathTextDecomposerPattern, System.Text.RegularExpressions.RegexOptions.Compiled);
                    }
                }
            }
            return _regex;
        }

        private static List<MathExpressionElement> DecomposeExpressionInElements(string originalMathExpression)
        {
            List<MathExpressionElement> listofElements = new List<MathExpressionElement>();
            var regexparser = GetParser();

            foreach (var mathExpression in SplitStringInChunks(originalMathExpression))
            {
                var match = regexparser.Match(mathExpression);
                while (match.Success)
                {
                    if (match.Groups["LiteralValue"].Success)
                    {
                        listofElements.Add(new(TypesOfElements.LiteralValue, match.Captures[0].Value));
                    }
                    if (match.Groups["Operator"].Success)
                    {
                        listofElements.Add(new(TypesOfElements.Operator, match.Captures[0].Value));
                    }
                    if (match.Groups["MathExpression"].Success)
                    {
                        listofElements.Add(new(TypesOfElements.Expression, match.Captures[0].Value));
                    }
                    match = match.NextMatch();
                }
            }

            if(IsThisASingleDelimitedExpression(listofElements))
            {
                string elementText = listofElements[0].TextualValue.Remove(0, 1);
                elementText = elementText.Remove(elementText.Length - 1, 1);
                listofElements = DecomposeExpressionInElements(elementText);
            }

            if (listofElements.Count == 1)
            {
                listofElements.Add(new(TypesOfElements.Operator, "+"));
                listofElements.Add(new(TypesOfElements.LiteralValue, "0"));
            }
            return listofElements;
        }

        private static bool IsThisASingleDelimitedExpression(List<MathExpressionElement> listofElements)
        {
            if (listofElements.Count == 1 && listofElements[0].TypeOfElement == TypesOfElements.Expression) return true;
            return false;
        }

        private static List<string> SplitStringInChunks(string originalExpression)
        {
            List<String> chunks = new List<string>();
            int delimitersCount = 0;
            int lastPosition = originalExpression.Length - 1;
            for (int i = 0; i <= lastPosition; i++)
            {
                if (originalExpression[i] == '(')
                {
                    if (i > 0)
                    {
                        chunks.Add(originalExpression.Substring(0, i));
                        chunks.AddRange(SplitStringInChunks(originalExpression.Substring(i)));
                        return chunks;
                    }
                    delimitersCount++;
                }
                if (originalExpression[i] == ')')
                {
                    delimitersCount--;
                    if (delimitersCount == 0)
                    {
                        chunks.Add(originalExpression.Substring(0, i + 1));
                        if (i < lastPosition) chunks.AddRange(SplitStringInChunks(originalExpression.Substring(i + 1)));
                        return chunks;
                    }
                }
            }
            chunks.Add(originalExpression);
            return chunks;
        }

        private static List<MathExpressionElement> AdjustImplicityNegativeMultiplierOperator(List<MathExpressionElement> listofElements)
        {
            List<MathExpressionElement> adjustedElements = new List<MathExpressionElement>();
            for (int i = 0; i <= listofElements.Count - 1; i++)
            {
                if(i == 0 && listofElements[i].TypeOfElement == TypesOfElements.Operator && listofElements[i].TextualValue == "-")
                {
                    adjustedElements.Add(new(TypesOfElements.LiteralValue, "-1"));
                    adjustedElements.Add(new(TypesOfElements.Operator, "*"));
                    continue;
                }

                if (i > 0)
                {
                    if (listofElements[i].TypeOfElement == TypesOfElements.Operator)
                    {
                        if (listofElements[i].TextualValue == "-")
                        {
                            if (listofElements[i - 1].TypeOfElement == TypesOfElements.Operator)
                            {
                                adjustedElements.Add(new(TypesOfElements.LiteralValue, "-1"));
                                adjustedElements.Add(new(TypesOfElements.Operator, "*"));
                                continue;
                            }
                        }
                    }
                }
                adjustedElements.Add(listofElements[i]);
            }
            return adjustedElements;
        }

        private static SimpleMathOperators GetMathOperator(string mathOperator)
        {
            switch (mathOperator)
            {
                case "+":
                    return SimpleMathOperators.Add;
                case "-":
                    return SimpleMathOperators.Sub;
                case "*":
                    return SimpleMathOperators.Multiply;
                case "/":
                    return SimpleMathOperators.Divide;
                default:
                    throw new Exception($"This operator [{mathOperator}] was not recognized.");
            }
        }

        private static string JoinExpressions(List<MathExpressionElement> listofElements)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in listofElements)
            {
                stringBuilder.Append(item.TextualValue);
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString().Trim();
        }

        private static (List<MathExpressionElement>, SimpleMathOperators, List<MathExpressionElement>) Split(List<MathExpressionElement> listofElements)
        {
            int nonPriorityOperatorIndex = GetNonPriorityOperatorIndex(listofElements);
            return SplitAt(listofElements, nonPriorityOperatorIndex);
        }

        private static int GetNonPriorityOperatorIndex(List<MathExpressionElement> listofElements)
        {
            for (int i = 0; i < listofElements.Count; i++)
            {
                if (listofElements[i].TypeOfElement != TypesOfElements.Operator) continue;
                SimpleMathOperators currentElementOperator = GetMathOperator(listofElements[i].TextualValue);
                if (currentElementOperator == SimpleMathOperators.Add || currentElementOperator == SimpleMathOperators.Sub)
                {
                    return i;
                }
            }

            for (int i = listofElements.Count - 1; i >= 0; i--)
            {
                if (listofElements[i].TypeOfElement != TypesOfElements.Operator) continue;
                SimpleMathOperators currentElementOperator = GetMathOperator(listofElements[i].TextualValue);
                if (currentElementOperator == SimpleMathOperators.Multiply || currentElementOperator == SimpleMathOperators.Divide)
                {
                    return i;
                }
            }
            return 1;
        }

        private static (List<MathExpressionElement>, SimpleMathOperators, List<MathExpressionElement>) SplitAt(List<MathExpressionElement> listofElements, int elementIndex)
        {
            List<MathExpressionElement> leftExpressions = new List<MathExpressionElement>();
            List<MathExpressionElement> rightExpressions = new List<MathExpressionElement>();

            SimpleMathOperators currentElementOperator = GetMathOperator(listofElements[elementIndex].TextualValue);

            for (int j = 0; j < elementIndex; j++)
            {
                leftExpressions.Add(listofElements[j]);
            }
            for (int k = elementIndex + 1; k < listofElements.Count; k++)
            {
                rightExpressions.Add(listofElements[k]);
            }
            return (leftExpressions, currentElementOperator, rightExpressions);
        }
    }
}
