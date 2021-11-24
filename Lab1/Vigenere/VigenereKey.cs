namespace security.Vigenere
{
    using System;
    using System.Text;
    using Genetic;

    public class VigenereKey : Key<VigenereKey>, IEquatable<VigenereKey>
    {
        public override int Length => alphabet.Length;

        private string alphabet;

        public VigenereKey(int length)
        {
            var repeat = (int)Math.Ceiling((double)length / Utils.NumberOfLetters);
            var line = new StringBuilder(length * repeat).Insert(0, Utils.EnglishAlphabet, repeat).ToString();
            alphabet = line.Substring(0, length);
        }

        public override void GetGenFrom(int index, VigenereKey key)
        {
            Set(index, key[index]);
        }

        public override void Mutate(int index)
        {
            var letter = Utils.GetRandomLetter();
            Set(index, letter);
        }

        private void Set(int index, char letter)
        {
            var builder = new StringBuilder(alphabet)
            {
                [index] = letter,
            };
            alphabet = builder.ToString();
        }


        public bool Equals(VigenereKey other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return alphabet == other.alphabet;
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
            return Equals((VigenereKey)obj);
        }

        public override int GetHashCode()
        {
            return alphabet.GetHashCode();
        }

        public static bool operator ==(VigenereKey left, VigenereKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VigenereKey left, VigenereKey right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return alphabet;
        }

        public char this[int i] => alphabet[i];
    }
}
