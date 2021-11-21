namespace security
{
    using System;
    using System.Collections;
    using System.Linq;

    public class Key : IEquatable<Key>
    {
        public int Length => bitArray.Length / 8;

        private readonly BitArray bitArray;

        public Key(Key key)
        {
            bitArray = new BitArray(key.bitArray);
        }

        public Key(int length)
        {
            bitArray = new BitArray(length * 8);
        }

        public Key(byte[] bytes)
        {
            bitArray = new BitArray(bytes);
        }

        public bool GetBit(int index)
        {
            return bitArray.Get(GetIndex(index));
        }

        public void SetBit(int index, bool bit)
        {
            bitArray.Set(GetIndex(index), bit);
        }

        private int GetIndex(int index)
        {
            return index;
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[Length];
            bitArray.CopyTo(bytes, 0);
            return bytes.ToArray();
        }

        public bool Equals(Key other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(bitArray, other.bitArray);
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
            return Equals((Key)obj);
        }

        public override int GetHashCode()
        {
            return (bitArray != null ? bitArray.GetHashCode() : 0);
        }

        public static bool operator ==(Key left, Key right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Key left, Key right)
        {
            return !Equals(left, right);
        }
    }
}
