namespace Challenges
{
    public class BalancedDelimitersChecker
    {
        public static void CheckForBalancedDelimiters(string stringInput)
        {
            if (string.IsNullOrWhiteSpace(stringInput)) throw new ArgumentException($"The parameter {nameof(stringInput)} is null or empty");

            Stack<State> stackedStates = new Stack<State>();
            char currentOpeningDelimiter = new char();
            var currentAlgoState = AlgoStates.SearchingForAnOpeningDelimiter;
            char currentCharacter;
            var totalCharacters = stringInput.Length;

            for (int i = 0; i < (totalCharacters - 1); i++)
            {
                currentCharacter = stringInput[i];
                switch (currentAlgoState)
                {
                    case AlgoStates.SearchingForAnOpeningDelimiter:
                        SearchForOpeningDelimiter(i, currentCharacter, ref currentAlgoState, ref currentOpeningDelimiter, stackedStates);
                        break;
                    case AlgoStates.SearchingForClosingDelimiter:
                        SearchForClosingDelimiter(i, currentCharacter, ref currentAlgoState, ref currentOpeningDelimiter, stackedStates);
                        break;
                    default:
                        throw new NotImplementedException("This support for this AlgoStates value is not implemented.");
                }
            }
            if (stackedStates.Count > 0) throw new UnbalancedDelimitersException("");
        }

        private static void SearchForOpeningDelimiter(int currentIndex, char currentCharacter, ref AlgoStates currentState, ref char currentOpeningDelimiter, Stack<State> stackedStates)
        {
            if (!IsAnOpeningDelimiter(currentCharacter)) throw new WrongCharacterTypeException(currentIndex, "Opening delimiter", currentCharacter);

            currentState = AlgoStates.SearchingForClosingDelimiter;
            currentOpeningDelimiter = currentCharacter;
        }

        private static void SearchForClosingDelimiter(int currentIndex, char currentCharacter, ref AlgoStates currentState, ref char currentOpeningDelimiter, Stack<State> stackedStates)
        {
            if (IsAnOpeningDelimiter(currentCharacter))
            {
                StackCurrentState(currentIndex, currentCharacter, ref currentOpeningDelimiter, stackedStates);
            }
            else
            {
                if (MatchDelimiters(currentOpeningDelimiter, currentCharacter) == false) throw new WrongClosingDelimiterCharacterException(currentIndex, "Closing delimiter", expected: currentOpeningDelimiter, found: currentCharacter);

                DestackCurrentState(currentIndex, ref currentOpeningDelimiter, stackedStates);
            }
        }

        private static void StackCurrentState(int currentIndex, char currentCharacter, ref char currentOpeningDelimiter, Stack<State> stackedStates)
        {
            var currentState = new State()
            {
                StartingPosition = currentIndex - 1,
                OpeningDelimiter = currentOpeningDelimiter,
            };

            stackedStates.Push(currentState);
            currentOpeningDelimiter = currentCharacter;
        }

        private static void DestackCurrentState(int currentIndex, ref char currentOpeningDelimiter, Stack<State> stackedStates)
        {
            var stackedState = stackedStates.Pop();
            currentOpeningDelimiter = stackedState.OpeningDelimiter;
        }

        private struct State
        {
            public int StartingPosition;
            public char OpeningDelimiter;
        }

        private static bool IsAnOpeningDelimiter(char charItem)
        {
            if (charItem == '(') return true;
            if (charItem == '{') return true;
            if (charItem == '[') return true;
            return false;
        }

        private static bool MatchDelimiters(char openingDelimiter, char closingDelimiter)
        {
            if (openingDelimiter == '(' && closingDelimiter == ')') return true;
            if (openingDelimiter == '{' && closingDelimiter == '}') return true;
            if (openingDelimiter == '[' && closingDelimiter == ']') return true;
            return false;
        }

        private enum AlgoStates : int
        {
            SearchingForAnOpeningDelimiter = 1,
            SearchingForClosingDelimiter = 2,
        }
    }
}