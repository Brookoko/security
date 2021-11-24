namespace security
{
    using Genetic;

    public class MultiByteDecipher : GeneticAlgorithm<MultiByteKey>
    {
        protected override int NumberOfIterations => 3000;
        protected override int PopulationSize => 100;
        protected override int BestSize => 20;
        protected override double MutationProbability => 0.5;
        protected override int NumberOfMutations => 30;

        private readonly MultiByteDecoder decoder;
        private readonly int keyLength;

        public MultiByteDecipher(int keyLength)
        {
            decoder = new MultiByteDecoder();
            this.keyLength = keyLength;
        }

        protected override MultiByteKey CreateEmpty()
        {
            return new MultiByteKey(keyLength);
        }

        protected override string Decode(string text, MultiByteKey key)
        {
            return decoder.Decode(text, key);
        }
    }
}
