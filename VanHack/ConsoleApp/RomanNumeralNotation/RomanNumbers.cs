using System.Collections.Generic;

namespace ConsoleApp.RomanNumeralNotation
{
    class RomanNumbers
    {
        public static string WriteInRomanNotation(int num)
        {
            return WriteNumberInRomanNotation(num, 1_000, 500, 100);
        }

        private static string WriteNumberInRomanNotation(int remainingValue, int value10x, int value5x, int value1x)
        {
            if (remainingValue == 0) return string.Empty;
            System.Text.StringBuilder numeralRomano = new System.Text.StringBuilder();
            int divByValue10x = remainingValue / value10x;

            for (int i = 0; i < divByValue10x; i++)
            {
                numeralRomano.Append(GetRomanNumeralFor(value10x));
                remainingValue = remainingValue - value10x;
            }

            if (remainingValue == 0) return numeralRomano.ToString();

            if (remainingValue >= (value10x - value1x))
            {
                numeralRomano.Append(GetRomanNumeralFor(value1x));
                numeralRomano.Append(GetRomanNumeralFor(value10x));
                remainingValue = remainingValue - (value10x - value1x);

                numeralRomano.Append(WriteNumberInRomanNotation(remainingValue, value10x / 10, value5x / 10, value1x / 10));
                return numeralRomano.ToString();
            }

            if (remainingValue >= (value5x + (3 * value1x)))
            {
                numeralRomano.Append(GetRomanNumeralFor(value5x));
                numeralRomano.Append(GetRomanNumeralFor(value1x));
                numeralRomano.Append(GetRomanNumeralFor(value1x));
                numeralRomano.Append(GetRomanNumeralFor(value1x));

                remainingValue = remainingValue - (value5x + (3 * value1x));

                numeralRomano.Append(WriteNumberInRomanNotation(remainingValue, value10x / 10, value5x / 10, value1x / 10));
                return numeralRomano.ToString();
            }

            if (remainingValue >= (value5x + (2 * value1x)))
            {
                numeralRomano.Append(GetRomanNumeralFor(value5x));
                numeralRomano.Append(GetRomanNumeralFor(value1x));
                numeralRomano.Append(GetRomanNumeralFor(value1x));

                remainingValue = remainingValue - (value5x + (2 * value1x));

                numeralRomano.Append(WriteNumberInRomanNotation(remainingValue, value10x / 10, value5x / 10, value1x / 10));
                return numeralRomano.ToString();
            }

            if (remainingValue >= (value5x + (1 * value1x)))
            {
                numeralRomano.Append(GetRomanNumeralFor(value5x));
                numeralRomano.Append(GetRomanNumeralFor(value1x));

                remainingValue = remainingValue - (value5x + (1 * value1x));

                numeralRomano.Append(WriteNumberInRomanNotation(remainingValue, value10x / 10, value5x / 10, value1x / 10));
                return numeralRomano.ToString();
            }

            if (remainingValue == value5x)
            {
                numeralRomano.Append(GetRomanNumeralFor(value5x));
                return numeralRomano.ToString();
            }

            if (remainingValue >= (value5x - (1 * value1x)))
            {
                numeralRomano.Append(GetRomanNumeralFor(value1x));
                numeralRomano.Append(GetRomanNumeralFor(value5x));

                remainingValue = remainingValue - (value5x - (1 * value1x));

                numeralRomano.Append(WriteNumberInRomanNotation(remainingValue, value10x / 10, value5x / 10, value1x / 10));
                return numeralRomano.ToString();
            }

            {
                numeralRomano.Append(WriteNumberInRomanNotation(remainingValue, value10x / 10, value5x / 10, value1x / 10));
                return numeralRomano.ToString();
            }
        }

        private static string GetRomanNumeralFor(int decimalValue)
        {
            switch (decimalValue)
            {
                case 1_000:
                    return "M";
                case 500:
                    return "D";
                case 100:
                    return "C";
                case 50:
                    return "L";
                case 10:
                    return "X";
                case 5:
                    return "V";
                case 1:
                    return "I";
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }

        public static int WriteDecimalFromRoman(string roman)
        {
            int decimalValue = 0;
            foreach (var romanBase in GetRomanBases())
            {
                if (romanBase.Value == 1000)
                {
                    do
                    {
                        if (roman.StartsWith(romanBase.Key))
                        {
                            decimalValue = decimalValue + romanBase.Value;
                            roman = roman.Remove(0, romanBase.Key.Length);
                        }
                        else
                        {
                            break;
                        }
                    } while (true);
                }

                if (roman.StartsWith(romanBase.Key))
                {
                    decimalValue = decimalValue + romanBase.Value;
                    roman = roman.Remove(0, romanBase.Key.Length);
                }
            }
            return decimalValue;
        }

        private static KeyValuePair<string, int>[] GetRomanBases()
        {
            List<KeyValuePair<string, int>> romanBases = new List<KeyValuePair<string, int>>();
            romanBases.Add(new KeyValuePair<string, int>("M", 1000));
            romanBases.Add(new KeyValuePair<string, int>("CM", 900));

            romanBases.Add(new KeyValuePair<string, int>("DCCC", 800));
            romanBases.Add(new KeyValuePair<string, int>("DCC", 700));
            romanBases.Add(new KeyValuePair<string, int>("DC", 600));
            romanBases.Add(new KeyValuePair<string, int>("D", 500));
            romanBases.Add(new KeyValuePair<string, int>("CD", 400));

            romanBases.Add(new KeyValuePair<string, int>("CCC", 300));
            romanBases.Add(new KeyValuePair<string, int>("CC", 200));
            romanBases.Add(new KeyValuePair<string, int>("C", 100));
            romanBases.Add(new KeyValuePair<string, int>("XC", 90));

            romanBases.Add(new KeyValuePair<string, int>("LXXX", 80));
            romanBases.Add(new KeyValuePair<string, int>("LXX", 70));
            romanBases.Add(new KeyValuePair<string, int>("LX", 60));
            romanBases.Add(new KeyValuePair<string, int>("L", 50));
            romanBases.Add(new KeyValuePair<string, int>("XL", 40));

            romanBases.Add(new KeyValuePair<string, int>("XXX", 30));
            romanBases.Add(new KeyValuePair<string, int>("XX", 20));
            romanBases.Add(new KeyValuePair<string, int>("X", 10));
            romanBases.Add(new KeyValuePair<string, int>("IX", 9));

            romanBases.Add(new KeyValuePair<string, int>("VIII", 8));
            romanBases.Add(new KeyValuePair<string, int>("VII", 7));
            romanBases.Add(new KeyValuePair<string, int>("VI", 6));
            romanBases.Add(new KeyValuePair<string, int>("V", 5));
            romanBases.Add(new KeyValuePair<string, int>("IV", 4));

            romanBases.Add(new KeyValuePair<string, int>("III", 3));
            romanBases.Add(new KeyValuePair<string, int>("II", 2));
            romanBases.Add(new KeyValuePair<string, int>("I", 1));

            return romanBases.ToArray();
        }
    }
}
