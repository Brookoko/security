namespace security.Substitution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SubstitutionDecipher
    {
        private const int NumberOfIterations = 3000;
        private const int PopulationSize = 1000;
        private const int BestSize = 20;
        private const double MutationProbability = 0.2;
        private const int NumberOfMutations = 1;

        private readonly Random random;
        private readonly SubstitutionDecoder decoder;
        private readonly FitnessCalculator fitnessCalculator;

        public SubstitutionDecipher()
        {
            random = new Random();
            decoder = new SubstitutionDecoder();
            fitnessCalculator = new FitnessCalculator();
        }

        public (string decipherText, SubstitutionKey key) Decipher(string text)
        {
            var keys = CreateInitialPopulation();
            var best = new SubstitutionKey();
            var count = 0;
            while (count < NumberOfIterations)
            {
                var (first, second) = Select(keys);
                var children = Crossover(first, second);
                Mutate(children);
                keys = keys.Take(BestSize).ToList();
                foreach (var child in children)
                {
                    if (!keys.Contains(child))
                    {
                        keys.Add(child);
                    }
                }
                keys = keys.OrderByDescending(k => CalculateScore(text, k)).Take(PopulationSize).ToList();
                best = keys[0];
                count++;
                Console.WriteLine($"{count}<>{best}<>{CalculateScore(text, best)}");
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
            for (var i = 0; i < Utils.NumberOfLetters; i++)
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
            for (var i = 0; i + 1 < keys.Count; i += 2)
            {
                first.Add(keys[i]);
                second.Add(keys[i + 1]);
            }
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
            var crossoverPoint = random.Next(Utils.NumberOfLetters);
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
            for (var i = 0; i < NumberOfMutations; i++)
            {
                var index = random.Next(Utils.NumberOfLetters);
                var letter = random.Next(Utils.NumberOfLetters);
                key.ReplaceWith(index, Utils.EnglishAlphabet[letter]);
            }
        }

        private double CalculateScore(string text, SubstitutionKey key)
        {
            var decipherText = decoder.Decode(text, key);
            return fitnessCalculator.Calculate(decipherText);
        }
    }
}
