namespace security
{
    using System.IO;
    using System.Linq;

    class Program
    {
        private static readonly StringEncoder StringEncoder = new();

        static void Main(string[] args)
        {
            FromBinaryToText();
            DecodeSingleByte();
        }

        private static void DecodeSingleByte()
        {
            var text = File.ReadAllText(GetPathInProject("lab1single.txt"));
            var bytes = StringEncoder.GetBytes(text);
            var results = new string[256];
            for (var i = 0; i < 256; i++)
            {
                var result = Xor(bytes, (byte)i);
                results[i] = StringEncoder.GetString(result);
            }
            var aggregate = "";
            for (var i = 0; i < results.Length; i++)
            {
                aggregate += $"{i}\n{results[i]}\n<---------->\n";
            }
            File.WriteAllText(GetPathInProject("lab1single-decoded.txt"), aggregate);
        }

        private static byte[] Xor(byte[] bytes, byte k)
        {
            return bytes.Select(b => (byte)(b ^ k)).ToArray();
        }

        private static string GetPathInProject(string path)
        {
            return $"../../../{path}";
        }

        private static void FromBinaryToText()
        {
            var text = File.ReadAllText(GetPathInProject("lab1.txt"));
            var bytes = StringEncoder.GetBytes(text);
            var decoded = StringEncoder.GetString(bytes);
            var baseString = StringEncoder.GetBytes(decoded);
            File.WriteAllBytes(GetPathInProject("lab1base-decoded.txt"), baseString);
        }
    }
}
