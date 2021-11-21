namespace security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Utils
    {
        public static string EnglishAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const double EnglishIC = 0.0667;
        public const int NumberOfLetters = 26;

        public static string GetClosestToEnglish(string[] variants)
        {
            return variants
                .OrderByDescending(EnglishLettersCoef)
                .ThenBy(DifferenceFromEnglish)
                .First();
        }

        public static double EnglishLettersCoef(string text)
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
            foreach (var (_, freq) in frequencies)
            {
                sum += freq * (freq - 1);
                total += freq;
            }
            if (total == 0)
            {
                return 0;
            }
            return sum / (total * (total - 1));
        }

        public static Dictionary<char, int> CalculateFrequency(string text)
        {
            var frequencies = new int[NumberOfLetters];
            foreach (var c in text.Where(IsEnglish))
            {
                frequencies[GetIndex(c)]++;
            }
            return frequencies
                .Select((i, index) => (i, index))
                .ToDictionary(p => GetLetter(p.index), p => p.i);

            char GetLetter(int i)
            {
                return (char)('A' + i);
            }
        }

        public static int GetIndex(char c)
        {
            return char.IsUpper(c) ? c - 'A' : c - 'a';
        }

        private static bool IsEnglish(char c)
        {
            return c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
        }
    }
}
