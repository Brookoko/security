namespace security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MultiByteDecipher
    {
        private const int PopulationSize = 10;
        private const double MutationProbability = 0.3;

        private readonly MultiByteDecoder decoder;
        private readonly Random random;
        private readonly int keyLength;
        private string text;

        public MultiByteDecipher(int keyLength)
        {
            this.keyLength = keyLength;
            random = new Random();
            decoder = new MultiByteDecoder();
        }

        public (string decipherText, MultiByteKey key) Decipher(string text)
        {
            this.text = text;
            var keys = CreateInitialPopulation();
            var generationWithoutChanges = 0;
            var best = new MultiByteKey(0);
            while (generationWithoutChanges < 1000)
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
                if (best == keys[0])
                {
                    generationWithoutChanges++;
                }
                else
                {
                    generationWithoutChanges = 0;
                }
                best = keys[0];
            }
            return (decoder.Decode(text, keys[0]), keys[0]);
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
            var chosen = new List<int>();
            for (var i = 0; i < keys.Count / 2; i++)
            {
                chosen.Add(i);
                first.Add(keys[i]);
                var pair = i;
                while (chosen.Contains(pair))
                {
                    pair = random.Next(i + 1, keys.Count);
                }
                second.Add(keys[pair]);
            }
            return (first, second);
        }

        private double CalculateScore(MultiByteKey key)
        {
            var decipherText = decoder.Decode(text, key);
            var frequencies = Utils.CalculateFrequency(decipherText);
            var score = 0.0;
            double total = decipherText.Length;
            foreach (var (c, freq) in frequencies)
            {
                score += Math.Abs(Frequencies.SingleLetterFrequency[c] - freq / total);
            }
            var ic = Utils.CalculateIC(decipherText);
            return 0.5 * (1 - Utils.EnglishLettersCoef(decipherText)) + 0.5 * score;
            // 0.4 * Math.Abs(Utils.EnglishIC - ic);
        }

        private List<MultiByteKey> Crossover(List<MultiByteKey> first, List<MultiByteKey> second)
        {
            var children = new List<MultiByteKey>();
            for (var i = 0; i < PopulationSize / 2; i++)
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
                    MutateSingle(key);
                }
            }
        }

        private void MutateSingle(MultiByteKey key)
        {
            var numberOfBits = random.Next(keyLength * 8);
            for (var i = 0; i < numberOfBits; i++)
            {
                var bit = random.Next(keyLength * 8);
                key.SetBit(bit, !key.GetBit(bit));
            }
        }
    }
}
