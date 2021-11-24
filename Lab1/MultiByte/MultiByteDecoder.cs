namespace security
{
    using System.Linq;

    public class MultiByteDecoder
    {
        private readonly StringEncoder stringEncoder = new();

        public string Decode(string text, MultiByteKey key)
        {
            var bytes = stringEncoder.GetBytes(text);
            var keyBytes = key.ToBytes();
            var decodedBytes = bytes.Select((b, i) => (byte)(b ^ keyBytes[i % key.ByteLength])).ToArray();
            return stringEncoder.GetString(decodedBytes);
        }
    }
}
