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

        protected override SubstitutionKey CreateEmpty()
        {
            return new SubstitutionKey();
        }

        protected override string Decode(string text, SubstitutionKey key)
        {
            return decoder.Decode(text, key);
        }
    }
}
