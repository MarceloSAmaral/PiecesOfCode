using System;

namespace ConsoleApp.SimpleMathExpressionsParser
{
    public class SimpleMathematicalExpression
    {
        public SimpleMathematicalExpression() { }
        public string TextualMathExpression { get; }

        public double LeftNodeValue
        {
            get
            {
                if (_LeftNodeLiteralValue.HasValue) return _LeftNodeLiteralValue.Value;
                _LeftNodeLiteralValue = LeftNodeExpression.Calc();
                return _LeftNodeLiteralValue.Value;
            }
        }

        public SimpleMathOperators MathOperator { get; set; }

        private double RightNodeValue
        {
            get
            {
                if (_RightNodeLiteralValue.HasValue) return _RightNodeLiteralValue.Value;
                _RightNodeLiteralValue = RightNodeExpression.Calc();
                return _RightNodeLiteralValue.Value;
            }
        }

        public double? _LeftNodeLiteralValue;
        public SimpleMathematicalExpression LeftNodeExpression;

        public double? _RightNodeLiteralValue;
        public SimpleMathematicalExpression RightNodeExpression;

        public double Calc()
        {
            switch (MathOperator)
            {
                case SimpleMathOperators.NotDefined:
                    throw new Exception("Operator is not defined");

                case SimpleMathOperators.Add:
                    return LeftNodeValue + RightNodeValue;

                case SimpleMathOperators.Sub:
                    return LeftNodeValue - RightNodeValue;

                case SimpleMathOperators.Multiply:
                    return LeftNodeValue * RightNodeValue;

                case SimpleMathOperators.Divide:
                    return LeftNodeValue / RightNodeValue;

                default:
                    throw new Exception("this operator is not supported.");
            }
        }
    }
}
