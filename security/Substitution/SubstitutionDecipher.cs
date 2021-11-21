namespace security.Substitution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class SubstitutionDecipher
    {
        private const int PopulationSize = 100;
        private const double MutationProbability = 0.3;
        private const int NumberOfMutations = 1;

        private readonly SubstitutionDecoder decoder;
        private readonly Random random;
        private string text;

        public SubstitutionDecipher()
        {
            random = new Random();
            decoder = new SubstitutionDecoder();
        }

        public (string decipherText, SubstitutionKey key) Decipher(string text)
        {
            this.text = text;
            var keys = CreateInitialPopulation();
            var best = new SubstitutionKey();
            var count = 0;
            while (count < 3000)
            {
                var (first, second) = Select(keys);
                var children = Crossover(first, second);
                Mutate(children);
                foreach (var child in children)
                {
                    if (!keys.Contains(child))
                    {
                        keys.Add(child);
                    }
                }
                keys = keys.OrderBy(CalculateScore).Take(PopulationSize).ToList();
                best = keys[0];
                count++;
                Console.WriteLine($"{count}<>{best}<>{CalculateScore(best)}");
            }
            foreach (var key in keys)
            {
                Console.WriteLine($"{key}\n<>\n{decoder.Decode(text, key)}\n<----->\n");
            }
            return (decoder.Decode(text, best), best);
        }

        private List<SubstitutionKey> CreateInitialPopulation()
        {
            var population = new List<SubstitutionKey>();
            for (var i = 0; i < PopulationSize; i++)
            {
                population.Add(CreateRandomKey());
            }
            return population;
        }

        private SubstitutionKey CreateRandomKey()
        {
            var key = new SubstitutionKey();
            for (var i = 0; i < 1; i++)
            {
                var letter = random.Next(Utils.NumberOfLetters);
                key.ReplaceWith(i, Utils.EnglishAlphabet[letter]);
            }
            return key;
        }

        private (List<SubstitutionKey> first, List<SubstitutionKey> second) Select(List<SubstitutionKey> keys)
        {
            var first = new List<SubstitutionKey>();
            var second = new List<SubstitutionKey>();
            var chosen = new List<int>();
            // for (var i = 0; i < keys.Count; i += 2)
            // {
            //     first.Add(keys[i]);
            //     chosen.Add(i);
            //     var next = i;
            //     while (chosen.Contains(next))
            //     {
            //         next = random.Next(i, keys.Count);
            //     }
            //     second.Add(keys[next]);
            // }
            for (var i = 0; i < keys.Count; i += 2)
            {
                first.Add(keys[i]);
                second.Add(keys[i + 1]);
            }
            // first.Add(keys[0]);
            // second.Add(keys[1]);
            return (first, second);
        }

        private List<SubstitutionKey> Crossover(List<SubstitutionKey> first, List<SubstitutionKey> second)
        {
            var children = new List<SubstitutionKey>();
            for (var i = 0; i < first.Count; i++)
            {
                var child = Crossover(first[i], second[i]);
                children.Add(child);
            }
            return children;
        }

        private SubstitutionKey Crossover(SubstitutionKey first, SubstitutionKey second)
        {
            var child = new SubstitutionKey();
            var crossoverPoint = Utils.NumberOfLetters / 2;
            var isFirst = random.Next(2) == 1;
            var firstDonor = isFirst ? first : second;
            var secondDonor = isFirst ? second : first;
            for (var i = 0; i < Utils.NumberOfLetters; i++)
            {
                var donor = i < crossoverPoint ? firstDonor : secondDonor;
                var c = donor[i];
                child.Set(i, c);
            }
            child.CorrectAlphabet();
            return child;
        }

        private void Mutate(List<SubstitutionKey> keys)
        {
            foreach (var key in keys)
            {
                if (random.NextDouble() < MutationProbability)
                {
                    Mutate(key);
                }
            }
        }

        private void Mutate(SubstitutionKey key)
        {
            // var numberOfMutations = random.Next(Utils.NumberOfLetters);
            for (var i = 0; i < NumberOfMutations; i++)
            {
                var index = random.Next(Utils.NumberOfLetters);
                var letter = random.Next(Utils.NumberOfLetters);
                key.ReplaceWith(index, Utils.EnglishAlphabet[letter]);
            }
            key.CorrectAlphabet();
        }

        private double CalculateScore(SubstitutionKey key)
        {
            var decipherText = decoder.Decode(text, key);
            return CalculateLetterScore(decipherText) +
                   CalculateBigramsScore(decipherText) +
                   CalculateTrigramsScore(decipherText);
            // return 0.5 * CalculateLetterScore(decipherText) +
            //        0.5 * CalculateIcScore(decipherText);
            // return CalculateLetterScore(decipherText) + CalculateBigramsScore(decipherText);
            // return CalculateBigramsScore(decipherText);
        }

        private double CalculateLetterScore(string text)
        {
            var frequencies = Utils.CalculateFrequency(text);
            var score = 0.0;
            double total = text.Length;
            foreach (var (c, freq) in frequencies)
            {
                score += Math.Abs(Frequencies.SingleLetterFrequency[c] - freq / total);
            }
            return score;
        }

        private double CalculateBigramsScore(string text)
        {
            return CalculateNgramsScore(text, Frequencies.BigramsFrequency);
        }

        private double CalculateTrigramsScore(string text)
        {
            return CalculateNgramsScore(text, Frequencies.TriramsFrequency);
        }

        private double CalculateNgramsScore(string text, Dictionary<string, double> grams)
        {
            var score = 0.0;
            var total = text.Length;
            foreach (var (gram, freq) in grams)
            {
                var count = CalculateSubstringCount(text, gram);
                score += Math.Abs(freq - (double)count / total);
            }
            return score;
        }

        private int CalculateSubstringCount(string text, string substring)
        {
            // var count = 0;
            // for (var i = 0; i < text.Length; i++)
            // {
            //     var length = Math.Min(text.Length - i - 1, substring.Length);
            //     var sub = text.Substring(i, length);
            //     if (sub == substring)
            //     {
            //         count++;
            //     }
            // }
            // return count;
            return Regex.Matches(text, substring).Count;
        }

        private double CalculateIcScore(string text)
        {
            var ic = Utils.CalculateIC(text);
            return Math.Abs(Utils.EnglishIC - ic);
        }
    }
}
