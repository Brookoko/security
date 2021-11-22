namespace security.Substitution
{
    using Genetic;

    public class SubstitutionDecipher : GeneticAlgorithm<SubstitutionKey>
    {
        protected override int NumberOfIterations => 3000;
        protected override int PopulationSize => 100;
        protected override int BestSize => 20;
        protected override double MutationProbability => 0.3;
        protected override int NumberOfMutations => 1;

        private readonly SubstitutionDecoder decoder;

        public SubstitutionDecipher()
        {
            decoder = new SubstitutionDecoder();
        }

        protected override SubstitutionKey CreateRandomKey()
        {
            var key = CreateEmpty();
            for (var i = 0; i < Utils.NumberOfLetters; i++)
            {
                var letter = random.Next(Utils.NumberOfLetters);
                key.ReplaceWith(i, Utils.EnglishAlphabet[letter]);
            }
            return key;
        }

        protected override SubstitutionKey CreateEmpty()
        {
            return new SubstitutionKey();
        }

        protected override SubstitutionKey Crossover(SubstitutionKey firstDonor, SubstitutionKey secondDonor,
            int crossoverPoint)
        {
            var child = CreateEmpty();
            for (var i = 0; i < Utils.NumberOfLetters; i++)
            {
                var donor = i < crossoverPoint ? firstDonor : secondDonor;
                var c = donor[i];
                child.Set(i, c);
            }
            child.CorrectAlphabet();
            return child;
        }

        protected override void Mutate(SubstitutionKey key, int numberOfMutations)
        {
            for (var i = 0; i < numberOfMutations; i++)
            {
                var index = random.Next(Utils.NumberOfLetters);
                var letter = random.Next(Utils.NumberOfLetters);
                key.ReplaceWith(index, Utils.EnglishAlphabet[letter]);
            }
        }

        protected override string Decode(string text, SubstitutionKey key)
        {
            return decoder.Decode(text, key);
        }
    }
}
