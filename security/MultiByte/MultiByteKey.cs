namespace security
{
    using System;
    using System.Collections;
    using System.Linq;

    public class MultiByteKey : IEquatable<MultiByteKey>
    {
        public int Length => bitArray.Length / 8;

        private readonly BitArray bitArray;

        public MultiByteKey(MultiByteKey multiByteKey)
        {
            bitArray = new BitArray(multiByteKey.bitArray);
        }

        public MultiByteKey(int length)
        {
            bitArray = new BitArray(length * 8);
        }

        public MultiByteKey(byte[] bytes)
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
    }
}
