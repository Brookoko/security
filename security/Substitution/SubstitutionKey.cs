namespace security.Substitution
{
    using System;
    using System.Linq;
    using System.Text;
    using Genetic;

    public class SubstitutionKey : Key<SubstitutionKey>, IEquatable<SubstitutionKey>
    {
        public override int Length => alphabet.Length;

        private string alphabet;

        public SubstitutionKey(string alphabet)
        {
            this.alphabet = alphabet;
        }

        public SubstitutionKey(SubstitutionKey key)
        {
            alphabet = key.alphabet;
        }

        public SubstitutionKey()
        {
            alphabet = Utils.EnglishAlphabet;
        }

        public override void GetGenFrom(int index, SubstitutionKey key)
        {
            var builder = new StringBuilder(alphabet)
            {
                [index] = key[index],
            };
            alphabet = builder.ToString();
        }

        public override void Mutate(int index)
        {
            var letter = Utils.GetRandomLetter();
            var temp = alphabet[index];
            var letterIndex = alphabet.IndexOf(letter);
            var builder = new StringBuilder(alphabet)
            {
                [index] = letter,
                [letterIndex] = temp
            };
            alphabet = builder.ToString();
        }

        public override void AfterCrossover()
        {
            var charsToInsert = Utils.EnglishAlphabet.Where(c => !alphabet.Contains(c)).ToList();
            var charsToRemove = alphabet
                .Select((c, i) => (c, i))
                .Where(p => alphabet.Substring(p.i + 1).Contains(p.c))
                .Select(p => p.c)
                .ToList();
            var builder = new StringBuilder(alphabet);
            foreach (var charToRemove in charsToRemove)
            {
                var index = alphabet.LastIndexOf(charToRemove);
                builder[index] = charsToInsert[0];
                charsToInsert.RemoveAt(0);
                alphabet = builder.ToString();
            }
        }

        public int IndexOf(char c)
        {
            return alphabet.IndexOf(c);
        }

        public bool Equals(SubstitutionKey other)
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
            return Equals((SubstitutionKey)obj);
        }

        public override int GetHashCode()
        {
            return alphabet.GetHashCode();
        }

        public static bool operator ==(SubstitutionKey left, SubstitutionKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SubstitutionKey left, SubstitutionKey right)
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
