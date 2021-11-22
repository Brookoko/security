namespace security.Vigenere
{
    using Genetic;

    public class VigenereDecipher : GeneticAlgorithm<VigenereKey>
    {
        protected override int NumberOfIterations => 3000;
        protected override int PopulationSize => 100;
        protected override int BestSize => 20;
        protected override double MutationProbability => 0.2;
        protected override int NumberOfMutations => 2;

        private readonly int keyLength;
        private readonly VigenereDecoder decoder;

        public VigenereDecipher(int keyLength)
        {
            this.keyLength = keyLength;
            decoder = new VigenereDecoder();
        }

        protected override VigenereKey CreateRandomKey()
        {
            var key = CreateEmpty();
            for (var i = 0; i < keyLength; i++)
            {
                var letter = random.Next(Utils.NumberOfLetters);
                key.Set(i, Utils.EnglishAlphabet[letter]);
            }
            return key;
        }

        protected override VigenereKey CreateEmpty()
        {
            return new VigenereKey(keyLength);
        }

        protected override VigenereKey Crossover(VigenereKey firstDonor, VigenereKey secondDonor, int crossoverPoint)
        {
            var child = CreateEmpty();
            for (var i = 0; i < keyLength; i++)
            {
                var donor = i < crossoverPoint ? firstDonor : secondDonor;
                var c = donor[i];
                child.Set(i, c);
            }
            return child;
        }

        protected override void Mutate(VigenereKey key, int numberOfMutations)
        {
            for (var i = 0; i < numberOfMutations; i++)
            {
                var index = random.Next(keyLength);
                var letter = random.Next(Utils.NumberOfLetters);
                key.Set(index, Utils.EnglishAlphabet[letter]);
            }
        }

        protected override string Decode(string text, VigenereKey key)
        {
            return decoder.Decode(text, key);
        }
    }
}
