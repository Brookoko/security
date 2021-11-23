namespace security.PolySubstitution
{
    using Genetic;

    public class PolySubstitutionDecipher : GeneticAlgorithm<PolySubstitutionKey>
    {
        protected override int NumberOfIterations => 8000;
        protected override int PopulationSize => 300;
        protected override int BestSize => 60;
        protected override double MutationProbability => 0.4;
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
