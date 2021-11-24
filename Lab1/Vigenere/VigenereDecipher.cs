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

        protected override VigenereKey CreateEmpty()
        {
            return new VigenereKey(keyLength);
        }

        protected override string Decode(string text, VigenereKey key)
        {
            return decoder.Decode(text, key);
        }
    }
}
