namespace security
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    class Program
    {
        static void Main(string[] args)
        {
            DecodeSingleByte();
        }

        private static void DecodeSingleByte()
        {
            var text = File.ReadAllText(GetPathInProject("lab1single.txt"));
            var bytes = ConvertHexToBytes(text);
            var results = new string[256];
            for (var i = 0; i < 256; i++)
            {
                var result = new byte[bytes.Length];
                for (var j = 0; j < bytes.Length; j++)
                {
                    result[j] = (byte)(bytes[j] ^ i);
                }
                var decipherText = Encoding.ASCII.GetString(result);
                results[i] = decipherText;
            }
            var aggregate = "";
            for (var i = 0; i < results.Length; i++)
            {
                aggregate += $"{i}\n{results[i]}\n<---------->\n";
            }
            File.WriteAllText(GetPathInProject($"lab1single-decoded.txt"), aggregate);
        }

        private static string GetPathInProject(string path)
        {
            return $"../../../{path}";
        }

        private static byte[] ConvertHexToBytes(string text)
        {
            return Enumerable.Range(0, text.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(text.Substring(x, 2), 16))
                .ToArray();
        }

        private static void FromBinaryToText()
        {
            var data = File.ReadAllText(GetPathInProject("lab1.txt"));
            var bytes = GetBytesFromBinaryString(data);
            var text = Encoding.ASCII.GetString(bytes);
            File.WriteAllText(GetPathInProject("lab1decoded.txt"), text);
        }

        public static byte[] GetBytesFromBinaryString(string binary)
        {
            var bytes = new List<byte>();
            for (var i = 0; i < binary.Length; i += 8)
            {
                if (i + 8 >= binary.Length)
                {
                    continue;
                }
                var substring = binary.Substring(i, 8);
                bytes.Add(Convert.ToByte(substring, 2));
            }
            return bytes.ToArray();
        }
    }
}
