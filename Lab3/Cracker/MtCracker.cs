namespace Lab3.Cracker
{
    public class MtCracker
    {
        public ulong[] GetState(long[] sequence)
        {
            var state = new ulong[624];
            for (var i = 0; i < 624; i++)
            {
                state[i] = (ulong)Reverse(sequence[i]);
            }
            return state;
        }

        private long Reverse(long value)
        {
            var v = value;
            v = UndoXorRightShift(v, 18);
            v = UndoXorLeftShiftMask(v, 15, 0xefc60000);
            v = UndoXorLeftShiftMask(v, 7, 0x9d2c5680);
            v = UndoXorRightShift(v, 11);
            return v;
        }

        private static long UndoXorRightShift(long x, int shift)
        {
            var res = x;

            for (var i = shift; i < 32; i += shift)
            {
                res ^= x >> i;
            }

            return res;
        }

        private static long UndoXorLeftShiftMask(long x, int shift, long mask)
        {
            var window = (1 << shift) - 1;
            var res = x;

            for (var i = 0; i < 32 / shift; i++)
            {
                res ^= ((window & res) << shift) & mask;
                window <<= shift;
            }

            return res;
        }
    }
}
