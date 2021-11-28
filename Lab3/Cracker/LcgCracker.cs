namespace Lab3.Cracker
{
    using System;
    using Generators;

    public class LcgCracker
    {
        public (long a, long b) CrackLcg(long[] sequence)
        {
            var x1 = sequence[0];
            var x2 = sequence[1];
            var x3 = sequence[2];
            var (inverse, sign) = ModularMultiplicativeInverse(x1 - x2, Lcg.M);
            var a = (sign * (x2 - x3) * inverse) % Lcg.M;
            var b = (x2 - a * x1) % Lcg.M;
            b %= Lcg.M;
            return (a, b);
        }

        private (long result, long sign) ModularMultiplicativeInverse(long n, long m)
        {
            var m0 = m;
            var y = 0L;
            var x = 1L;
            var sign = n / Math.Abs(n);
            n = Math.Abs(n);

            if (m == 1)
            {
                return (0, 1);
            }

            while (n > 1)
            {
                var q = n / m;

                var t = m;

                m = n % m;
                n = t;
                t = y;

                y = x - q * y;
                x = t;
            }

            if (x < 0)
            {
                x += m0;
            }

            return (x, sign);
        }
    }
}
