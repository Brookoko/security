namespace security
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    class Program
    {
        static void Main(string[] args)
        {
            DecodeSingleByte();
        }

        private static void DecodeSingleByte()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var text = File.ReadAllText(GetPathInProject("lab1single.txt"));
            var bytes = Encoding.ASCII.GetBytes(text);
            for (var i = 0; i < 256; i++)
            {
                var result = new byte[bytes.Length];
                for (var j = 0; j < bytes.Length; j++)
                {
                    result[j] = (byte)(bytes[j] ^ i);
                }
                var decipherText = Encoding.ASCII.GetString(result);
                var file = $"{i}".PadLeft(3, '0');
                File.WriteAllText(GetPathInProject($"results/{file}.txt"), decipherText);
            }
        }

        private static void FromBinaryToText()
        {
            var data = File.ReadAllText(GetPathInProject("lab1.txt"));
            var bytes = GetBytesFromBinaryString(data);
            var text = Encoding.ASCII.GetString(bytes);
            File.WriteAllText(GetPathInProject("lab1decoded.txt"), text);
        }

        private static string GetPathInProject(string path)
        {
            return $"../../../{path}";
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
