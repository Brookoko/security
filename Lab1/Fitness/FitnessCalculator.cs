namespace security
{
    using System.Collections.Generic;

    public class FitnessCalculator
    {
        public double Calculate(string text)
        {
            return 0.2 * CalculateUnigramsScore(text) +
                   0.3 * CalculateBigramsScore(text) +
                   0.5 * CalculateTrigramsScore(text);
        }

        private double CalculateUnigramsScore(string text)
        {
            return CalculateNgramsScore(text, Frequencies.UnigramsFrequency, Frequencies.UniramsFloor, 1);
        }

        private double CalculateBigramsScore(string text)
        {
            return CalculateNgramsScore(text, Frequencies.BigramsFrequency, Frequencies.BiramsFloor, 2);
        }

        private double CalculateTrigramsScore(string text)
        {
            return CalculateNgramsScore(text, Frequencies.TriramsFrequency, Frequencies.TriramsFloor, 3);
        }

        private double CalculateNgramsScore(string text, Dictionary<string, double> grams, double floor, int length)
        {
            var score = 0.0;
            for (var i = 0; i < text.Length - length - 1; i++)
            {
                var sub = text.Substring(i, length).ToUpper();
                if (grams.TryGetValue(sub, out var freq))
                {
                    score += freq;
                }
                else
                {
                    score += floor;
                }
            }
            return score;
        }
    }
}
