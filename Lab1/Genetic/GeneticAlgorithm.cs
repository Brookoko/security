namespace security.Genetic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class GeneticAlgorithm<T> where T : Key<T>
    {
        protected abstract int NumberOfIterations { get; }
        protected abstract int PopulationSize { get; }
        protected abstract int BestSize { get; }
        protected abstract double MutationProbability { get; }
        protected abstract int NumberOfMutations { get; }

        protected readonly Random random = new();
        private readonly FitnessCalculator fitnessCalculator = new();

        public (string decipherText, T key) Decipher(string text)
        {
            var keys = CreateInitialPopulation();
            var best = CreateEmpty();
            var count = 0;
            while (count < NumberOfIterations)
            {
                var (first, second) = Select(keys);
                var children = Crossover(first, second);
                Mutate(children);
                keys = keys.Take(BestSize).ToList();
                AddUnique(keys, children);
                keys = keys.OrderByDescending(k => CalculateScore(text, k)).Take(PopulationSize).ToList();
                best = keys[0];
                count++;
                Console.WriteLine($"{count}<>{best}<>{CalculateScore(text, best)}");
            }
            foreach (var key in keys)
            {
                Console.WriteLine($"{key}\n<>\n{Decode(text, key)}\n<----->\n");
            }
            return (Decode(text, best), best);
        }

        private void AddUnique(List<T> destination, List<T> source)
        {
            foreach (var child in source)
            {
                if (!destination.Contains(child))
                {
                    destination.Add(child);
                }
            }
        }

        private List<T> CreateInitialPopulation()
        {
            var population = new List<T>();
            for (var i = 0; i < PopulationSize; i++)
            {
                population.Add(CreateRandomKey());
            }
            return population;
        }

        private T CreateRandomKey()
        {
            var key = CreateEmpty();
            for (var i = 0; i < key.Length; i++)
            {
                key.Mutate(i);
            }
            return key;
        }

        protected abstract T CreateEmpty();

        private (List<T> first, List<T> second) Select(List<T> keys)
        {
            var first = new List<T>();
            var second = new List<T>();
            for (var i = 0; i + 1 < keys.Count; i += 2)
            {
                first.Add(keys[i]);
                second.Add(keys[i + 1]);
            }
            return (first, second);
        }

        private List<T> Crossover(List<T> first, List<T> second)
        {
            var children = new List<T>();
            for (var i = 0; i < first.Count; i++)
            {
                var child = Crossover(first[i], second[i]);
                children.Add(child);
            }
            return children;
        }

        private T Crossover(T first, T second)
        {
            var crossoverPoint = random.Next(first.Length);
            var isFirst = random.Next(2) == 1;
            var firstDonor = isFirst ? first : second;
            var secondDonor = isFirst ? second : first;
            var child = CreateEmpty();
            for (var i = 0; i < child.Length; i++)
            {
                var donor = i < crossoverPoint ? firstDonor : secondDonor;
                child.GetGenFrom(i, donor);
            }
            child.AfterCrossover();
            return child;
        }

        private void Mutate(List<T> keys)
        {
            foreach (var key in keys)
            {
                if (random.NextDouble() < MutationProbability)
                {
                    Mutate(key);
                }
            }
        }

        private void Mutate(T key)
        {
            for (var i = 0; i < NumberOfMutations; i++)
            {
                var index = random.Next(key.Length);
                key.Mutate(index);
            }
        }

        private double CalculateScore(string text, T key)
        {
            var decipherText = Decode(text, key);
            return fitnessCalculator.Calculate(decipherText);
        }

        protected abstract string Decode(string text, T key);
    }
}
