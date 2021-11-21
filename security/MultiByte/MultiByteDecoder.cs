namespace security
{
    using System.Linq;
    using System.Text;

    public class MultiByteDecoder
    {
        private readonly StringEncoder stringEncoder = new();

        public string Decode(string text, string key)
        {
            var bytes = Encoding.UTF8.GetBytes(key);
            return Decode(text, new MultiByteKey(bytes));
        }

        public string Decode(string text, MultiByteKey key)
        {
            var bytes = stringEncoder.GetBytes(text);
            var keyBytes = key.ToBytes();
            var decodedBytes = bytes.Select((b, i) => (byte)(b ^ keyBytes[i % key.Length])).ToArray();
            return stringEncoder.GetString(decodedBytes);
        }
    }
}
