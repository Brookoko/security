namespace Lab3.Generators
{
    public class Lcg
    {
        public const long M = 4294967296;

        private readonly long a;
        private readonly long b;

        private long last;

        public Lcg(long a, long b, long seed)
        {
            this.a = a;
            this.b = b;
            last = seed;
        }

        public int Next()
        {
            var n = NextLong();
            return (int)n;
        }

        public long NextLong()
        {
            last = (a * last + b) % M;
            return last;
        }
    }
}
