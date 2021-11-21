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

        public (string decipherText, Key key) Decipher(string text)
        {
            this.text = text;
            var keys = CreateInitialPopulation();
            var stringEncoder = new StringEncoder();
            var generationWithoutChanges = 0;
            var best = new Key(0);
            while (generationWithoutChanges < 100)
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
                best = keys[0];
            }
            foreach (var key in keys)
            {
                Console.WriteLine($"{stringEncoder.GetString(key.ToBytes())}\n{decoder.Decode(text, key)}\n<----->");
            }
            return (decoder.Decode(text, keys[0]), keys[0]);
        }

        private List<Key> CreateInitialPopulation()
        {
            var population = new List<Key>();
            for (var i = 0; i < PopulationSize; i++)
            {
                population.Add(CreateRandomKey());
            }
            return population;
        }

        private Key CreateRandomKey()
        {
            var key = new Key(keyLength);
            for (var i = 0; i < keyLength * 8; i++)
            {
                if ((i + 1) % 8 == 0)
                {
                    continue;
                }
                key.SetBit(i, random.Next(0, 2) == 1);
            }
            return key;
        }

        private (List<Key> first, List<Key> second) Select(List<Key> keys)
        {
            var first = new List<Key>();
            var second = new List<Key>();
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

        private double CalculateScore(Key key)
        {
            var decipherText = decoder.Decode(text, key);
            var frequencies = Utils.CalculateFrequency(decipherText);
            var score = 0.0;
            double total = frequencies.Sum(p => p.Value);
            foreach (var (c, freq) in frequencies)
            {
                score += Math.Abs(Frequencies.SingleLetterFrequency[c] - freq / total);
            }
            var ic = Utils.CalculateIC(decipherText);
            return 0.2 * (1 - Utils.EnglishLettersCoef(decipherText)) + 0.4 * score +
                   0.4 * Math.Abs(Utils.EnglishIC - ic);
        }

        private List<Key> Crossover(List<Key> first, List<Key> second)
        {
            var children = new List<Key>();
            for (var i = 0; i < PopulationSize / 2; i++)
            {
                var (firstChild, secondChild) = Crossover(first[i], second[i]);
                children.Add(firstChild);
                children.Add(secondChild);
            }
            return children;
        }

        private (Key first, Key second) Crossover(Key first, Key second)
        {
            var firstChild = new Key(first);
            var secondChild = new Key(second);
            var crossoverPoint = random.Next(keyLength * 8);
            for (var i = 0; i < crossoverPoint; i++)
            {
                if ((i + 1) % 8 == 0)
                {
                    continue;
                }
                firstChild.SetBit(i, second.GetBit(i));
                secondChild.SetBit(i, first.GetBit(i));
            }
            return (firstChild, secondChild);
        }

        private void Mutate(List<Key> keys)
        {
            foreach (var key in keys)
            {
                if (random.NextDouble() < MutationProbability)
                {
                    MutateSingle(key);
                }
            }
        }

        private void MutateSingle(Key key)
        {
            var numberOfBits = random.Next(keyLength * 8);
            for (var i = 0; i < numberOfBits; i++)
            {
                var bit = random.Next(keyLength * 8);
                if ((bit + 1) % 8 == 0)
                {
                    continue;
                }
                key.SetBit(bit, !key.GetBit(bit));
            }
        }
    }
}
