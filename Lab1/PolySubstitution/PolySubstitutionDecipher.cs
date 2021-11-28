namespace security.PolySubstitution
{
    using Genetic;

    public class PolySubstitutionDecipher : GeneticAlgorithm<PolySubstitutionKey>
    {
        protected override int NumberOfIterations => 10000;
        protected override int PopulationSize => 500;
        protected override int BestSize => 100;
        protected override double MutationProbability => 0.3;
        protected override int NumberOfMutations => 2;

        private readonly int keyLength;
        private readonly PolySubstitutionDecoder decoder;

        public PolySubstitutionDecipher(int keyLength)
        {
            this.keyLength = keyLength;
            decoder = new PolySubstitutionDecoder();
        }

        protected override PolySubstitutionKey CreateEmpty()
        {
            return new PolySubstitutionKey(keyLength);
        }

        protected override string Decode(string text, PolySubstitutionKey key)
        {
            return decoder.Decode(text, key);
        }
    }
}
