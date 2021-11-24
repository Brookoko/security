namespace security
{
    using System;
    using System.Linq;
    using System.Text;

    public class StringEncoder
    {
        public string GetString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public byte[] GetBytes(string text)
        {
            if (IsBinary(text))
            {
                return FromBinary(text);
            }
            if (IsHex(text))
            {
                return FromHex(text);
            }
            var buffer = new Span<byte>(new byte[text.Length]);
            if (Convert.TryFromBase64String(text, buffer, out var parsed))
            {
                return buffer.ToArray().Take(parsed).ToArray();
            }
            return FromSimpleText(text);
        }

        private bool IsBinary(string text)
        {
            return text.All(c => c is '0' or '1');
        }

        private byte[] FromBinary(string text)
        {
            return Enumerable.Range(0, text.Length)
                .Where(x => x % 8 == 0)
                .Select(x => Convert.ToByte(text.Substring(x, 8), 2))
                .ToArray();
        }

        private bool IsHex(string text)
        {
            return text.All(c => c is >= '0' and <= '9' or >= 'a' and <= 'f');
        }

        private byte[] FromHex(string text)
        {
            return Enumerable.Range(0, text.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(text.Substring(x, 2), 16))
                .ToArray();
        }

        private byte[] FromSimpleText(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }
    }
}
