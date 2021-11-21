namespace security
{
    using System;
    using System.IO;
    using System.Linq;
    using Substitution;

    class Program
    {
        private static readonly StringEncoder StringEncoder = new();

        static void Main(string[] args)
        {
            // FromBinaryToText();
            // DecodeSingleByte();
            // DecodeMultiByte();
            DecodeSubstitution();
        }

        private static void DecodeSubstitution()
        {
            var text = File.ReadAllText(GetPathInProject("lab1sub.txt"));
            var decipher = new SubstitutionDecipher();
            var (decipherText, key) = decipher.Decipher(text);
            var testKey = new SubstitutionKey("EOCMLGDQVIBTFWYHZUSPANKXRJ");
            var decoder = new SubstitutionDecoder();
            Console.WriteLine($"{key}");
            Console.WriteLine($"\n\n{decipherText}");
            Console.WriteLine($"\n\n{decoder.Decode(text, testKey)}");
        }

        private static void DecodeMultiByte()
        {
            var cypherText = File.ReadAllText(GetPathInProject("lab1multi.txt"));
            var bytes = StringEncoder.GetBytes(cypherText);
            var text = StringEncoder.GetString(bytes);
            var keyGuesser = new KeyGuesser();
            var length = keyGuesser.GetProbableKeyLength(text);
            var multiByteDecipher = new MultiByteDecipher(length);
            var (decipherText, key) = multiByteDecipher.Decipher(text);
            Console.WriteLine($"{length}");
            Console.WriteLine($"{StringEncoder.GetString(key.ToBytes())}");
            Console.WriteLine($"{decipherText}");
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
            File.WriteAllText(GetPathInProject("lab1single-decoded-all.txt"), aggregate);
            var possibleResult = Utils.GetClosestToEnglish(results);
            Console.WriteLine(possibleResult);
            File.WriteAllText(GetPathInProject("lab1single-decoded-possible.txt"), possibleResult);
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
