namespace security.PolySubstitution
{
    using System;
    using System.Linq;
    using Genetic;
    using Substitution;

    public class PolySubstitutionKey : Key<PolySubstitutionKey>, IEquatable<PolySubstitutionKey>
    {
        public override int Length => alphabets.Length * alphabets[0].Length;

        private readonly SubstitutionKey[] alphabets;

        public PolySubstitutionKey(int length)
        {
            alphabets = Enumerable.Range(0, length)
                .Select(_ => new SubstitutionKey())
                .ToArray();
        }

        public override void GetGenFrom(int index, PolySubstitutionKey key)
        {
            var (keyIndex, subIndex) = Nest(index);
            this[keyIndex].GetGenFrom(subIndex, key[keyIndex]);
        }

        public override void AfterCrossover()
        {
            base.AfterCrossover();
            foreach (var key in alphabets)
            {
                key.AfterCrossover();
            }
        }

        public override void Mutate(int index)
        {
            var (keyIndex, subIndex) = Nest(index);
            this[keyIndex].Mutate(subIndex);
        }

        private (int keyIndex, int subIndex) Nest(int index)
        {
            return (index / alphabets[0].Length, index % alphabets[0].Length);
        }

        public bool Equals(PolySubstitutionKey other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (Length != other.Length)
            {
                return false;
            }
            for (var i = 0; i < alphabets.Length; i++)
            {
                if (!alphabets[i].Equals(other.alphabets[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((PolySubstitutionKey)obj);
        }

        public override int GetHashCode()
        {
            return alphabets.GetHashCode();
        }

        public static bool operator ==(PolySubstitutionKey left, PolySubstitutionKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PolySubstitutionKey left, PolySubstitutionKey right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Join("\n", alphabets.Select(k => k.ToString()));
        }

        public SubstitutionKey this[int i] => alphabets[i];
    }
}
