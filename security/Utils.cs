namespace security
{
    using System;
    using System.Linq;

    public class Utils
    {
        private const int NumberOfLetters = 26;
        private const double EnglishIC = 1.73;

        public static string GetClosestToEnglish(string[] variants)
        {
            return variants
                .OrderByDescending(EnglishLettersCoef)
                .ThenBy(DifferenceFromEnglish)
                .First();
        }

        private static double EnglishLettersCoef(string text)
        {
            return (double)text.Where(IsEnglish).Count() / text.Length;
        }

        private static double DifferenceFromEnglish(string text)
        {
            return Math.Abs(EnglishIC - CalculateIC(text));
        }

        public static double CalculateIC(string text)
        {
            var frequencies = CalculateFrequency(text);
            var total = 0d;
            var sum = 0d;
            for (var i = 0; i < NumberOfLetters; i++)
            {
                sum += frequencies[i] * (frequencies[i] - 1);
                total += frequencies[i];
            }
            return sum / (total * (total - 1) / NumberOfLetters);
        }

        private static int[] CalculateFrequency(string text)
        {
            var frequencies = new int[NumberOfLetters];
            foreach (var c in text.Where(IsEnglish))
            {
                frequencies[GetIndex(c)]++;
            }
            return frequencies;

            int GetIndex(char c)
            {
                return char.IsUpper(c) ? c - 'A' : c - 'a';
            }
        }

        private static bool IsEnglish(char c)
        {
            return c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
        }
    }
}
