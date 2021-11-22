namespace security
{
    using Genetic;

    public class MultiByteDecipher : GeneticAlgorithm<MultiByteKey>
    {
        protected override int NumberOfIterations => 3000;
        protected override int PopulationSize => 100;
        protected override int BestSize => 20;
        protected override double MutationProbability => 0.5;
        protected override int NumberOfMutations => 5;
        
        private readonly MultiByteDecoder decoder;
        private readonly int keyLength;

        public MultiByteDecipher(int keyLength)
        {
            decoder = new MultiByteDecoder();
            this.keyLength = keyLength;
        }

        protected override MultiByteKey CreateRandomKey()
        {
            var key = CreateEmpty();
            for (var i = 0; i < keyLength * 8; i++)
            {
                key.SetBit(i, random.Next(0, 2) == 1);
            }
            return key;
        }

        protected override MultiByteKey CreateEmpty()
        {
            return new MultiByteKey(keyLength);
        }

        protected override MultiByteKey Crossover(MultiByteKey firstDonor, MultiByteKey secondDonor, int crossoverPoint)
        {
            var child = CreateEmpty();
            for (var i = 0; i < keyLength * 8; i++)
            {
                var donor = i < crossoverPoint ? firstDonor : secondDonor;
                child.SetBit(i, donor.GetBit(i));
            }
            return child;
        }

        protected override void Mutate(MultiByteKey key, int numberOfMutations)
        {
            for (var i = 0; i < numberOfMutations; i++)
            {
                var bit = random.Next(keyLength * 8);
                key.SetBit(bit, !key.GetBit(bit));
            }
        }

        protected override string Decode(string text, MultiByteKey key)
        {
            return decoder.Decode(text, key);
        }
    }
}
