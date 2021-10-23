namespace ConsoleApp.SimpleMathExpressionsParser
{
    public class MathExpressionElement
    {
        public MathExpressionElement(TypesOfElements typeOfElement, string textValue)
        {
            TypeOfElement = typeOfElement;
            TextualValue = textValue;
        }
        public TypesOfElements TypeOfElement;
        public string TextualValue;
    }
}
