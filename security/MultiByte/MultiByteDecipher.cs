namespace security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MultiByteDecipher
    {
        private const int NumberOfIterations = 3000;
        private const int PopulationSize = 100;
        private const int BestSize = 20;
        private const double MutationProbability = 0.3;
        private const int NumberOfMutations = 8;

        private readonly MultiByteDecoder decoder;
        private readonly Random random;
        private readonly FitnessCalculator fitnessCalculator;
        private readonly StringEncoder stringEncoder;
        private readonly int keyLength;

        private string text;

        public MultiByteDecipher(int keyLength)
        {
            random = new Random();
            decoder = new MultiByteDecoder();
            fitnessCalculator = new FitnessCalculator();
            stringEncoder = new StringEncoder();
            this.keyLength = keyLength;
        }

        public (string decipherText, MultiByteKey key) Decipher(string text)
        {
            this.text = text;
            var keys = CreateInitialPopulation();
            var best = new MultiByteKey(0);
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
                keys = keys.OrderByDescending(CalculateScore).Take(PopulationSize).ToList();
                best = keys[0];
                count++;
                Console.WriteLine($"{count}<>{stringEncoder.GetString(best.ToBytes())}<>{CalculateScore(best)}");
            }
            foreach (var key in keys)
            {
                Console.WriteLine(
                    $"{stringEncoder.GetString(key.ToBytes())}\n<>\n{decoder.Decode(text, key)}\n<----->\n");
            }
            return (decoder.Decode(text, best), best);
        }

        private List<MultiByteKey> CreateInitialPopulation()
        {
            var population = new List<MultiByteKey>();
            for (var i = 0; i < PopulationSize; i++)
            {
                population.Add(CreateRandomKey());
            }
            return population;
        }

        private MultiByteKey CreateRandomKey()
        {
            var key = new MultiByteKey(keyLength);
            for (var i = 0; i < keyLength * 8; i++)
            {
                key.SetBit(i, random.Next(0, 2) == 1);
            }
            return key;
        }

        private (List<MultiByteKey> first, List<MultiByteKey> second) Select(List<MultiByteKey> keys)
        {
            var first = new List<MultiByteKey>();
            var second = new List<MultiByteKey>();
            for (var i = 0; i + 1 < keys.Count; i += 2)
            {
                first.Add(keys[i]);
                second.Add(keys[i + 1]);
            }
            return (first, second);
        }

        private List<MultiByteKey> Crossover(List<MultiByteKey> first, List<MultiByteKey> second)
        {
            var children = new List<MultiByteKey>();
            for (var i = 0; i < first.Count; i++)
            {
                var (firstChild, secondChild) = Crossover(first[i], second[i]);
                children.Add(firstChild);
                children.Add(secondChild);
            }
            return children;
        }

        private (MultiByteKey first, MultiByteKey second) Crossover(MultiByteKey first, MultiByteKey second)
        {
            var firstChild = new MultiByteKey(first);
            var secondChild = new MultiByteKey(second);
            var crossoverPoint = random.Next(keyLength * 8);
            for (var i = 0; i < keyLength * 8; i++)
            {
                var firstDonor = i < crossoverPoint ? first : second;
                var secondDonor = i < crossoverPoint ? second : first;
                firstChild.SetBit(i, secondDonor.GetBit(i));
                secondChild.SetBit(i, firstDonor.GetBit(i));
            }
            return (firstChild, secondChild);
        }

        private void Mutate(List<MultiByteKey> keys)
        {
            foreach (var key in keys)
            {
                if (random.NextDouble() < MutationProbability)
                {
                    Mutate(key);
                }
            }
        }

        private void Mutate(MultiByteKey key)
        {
            for (var i = 0; i < NumberOfMutations; i++)
            {
                var bit = random.Next(keyLength * 8);
                key.SetBit(bit, !key.GetBit(bit));
            }
        }

        private double CalculateScore(MultiByteKey key)
        {
            var decipherText = decoder.Decode(text, key);
            var englishCoef = 1 - Utils.EnglishLettersCoef(decipherText);
            return fitnessCalculator.Calculate(decipherText) * englishCoef;
        }
    }
}
