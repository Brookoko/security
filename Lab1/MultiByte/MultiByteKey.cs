namespace security
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using Genetic;

    public class MultiByteKey : Key<MultiByteKey>, IEquatable<MultiByteKey>
    {
        public override int Length => bitArray.Length;

        public int ByteLength => bitArray.Length / 8;

        private readonly BitArray bitArray;

        public MultiByteKey(int length)
        {
            bitArray = new BitArray(length * 8);
        }

        public override void GetGenFrom(int index, MultiByteKey key)
        {
            this[index] = key[index];
        }

        public override void Mutate(int index)
        {
            this[index] = !this[index];
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[ByteLength];
            bitArray.CopyTo(bytes, 0);
            return bytes.ToArray();
        }

        public bool Equals(MultiByteKey other)
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
            for (var i = 0; i < bitArray.Count; i++)
            {
                if (bitArray[i] != other.bitArray[i])
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
            return Equals((MultiByteKey)obj);
        }

        public override int GetHashCode()
        {
            return (bitArray != null ? bitArray.GetHashCode() : 0);
        }

        public static bool operator ==(MultiByteKey left, MultiByteKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MultiByteKey left, MultiByteKey right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(ToBytes());
        }

        public bool this[int i]
        {
            get => bitArray[i];
            set => bitArray[i] = value;
        }
    }
}
