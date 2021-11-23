namespace security
{
    using System;
    using System.IO;
    using System.Linq;
    using Genetic;
    using Substitution;
    using Vigenere;

    class Program
    {
        private static readonly StringEncoder StringEncoder = new();

        static void Main(string[] args)
        {
            // FromBinaryToText();
            // DecodeSingleByte();
            DecodeMultiByte();
            // DecodeSubstitution();
            // DecodeVigenere();
        }

        private static void DecodeVigenere()
        {
            var text = File.ReadAllText(GetPathInProject("data/task5.txt"));
            var keyGuesser = new KeyGuesser();
            var length = keyGuesser.GetProbableKeyLength(text);
            var decipher = new VigenereDecipher(4);
            Console.WriteLine($"{length}");
            var (decipherText, key) = decipher.Decipher(text);
            WriteResult(key, decipherText, "task5.txt");
        }

        private static void DecodeSubstitution()
        {
            var text = File.ReadAllText(GetPathInProject("data/task4.txt"));
            var decipher = new SubstitutionDecipher();
            var (decipherText, key) = decipher.Decipher(text);
            WriteResult(key, decipherText, "task4.txt");
        }

        private static void DecodeMultiByte()
        {
            var cypherText = File.ReadAllText(GetPathInProject("data/task3.txt"));
            var bytes = StringEncoder.GetBytes(cypherText);
            var text = StringEncoder.GetString(bytes);
            var keyGuesser = new KeyGuesser();
            var length = keyGuesser.GetProbableKeyLength(text);
            var multiByteDecipher = new MultiByteDecipher(length);
            var (decipherText, key) = multiByteDecipher.Decipher(text);
            WriteResult(key, decipherText, "task3.txt");
        }

        private static void WriteResult<T>(Key<T> key, string text, string path) where T : Key<T>
        {
            var result = $"{key}\n{text}";
            File.WriteAllText(GetPathInProject($"results/{path}"), result);
        }

        private static void DecodeSingleByte()
        {
            var text = File.ReadAllText(GetPathInProject("data/task2.txt"));
            var bytes = StringEncoder.GetBytes(text);
            var results = new string[256];
            for (var i = 0; i < 256; i++)
            {
                var result = Xor(bytes, (byte)i);
                results[i] = StringEncoder.GetString(result);
            }
            WriteAllResults(results);
            WriteMostPossibleResult(results);
        }

        private static void WriteAllResults(string[] results)
        {
            var aggregate = "";
            for (var i = 0; i < results.Length; i++)
            {
                aggregate += $"{i}\n{results[i]}\n<---------->\n";
            }
            File.WriteAllText(GetPathInProject("results/task2-all.txt"), aggregate);
        }

        private static void WriteMostPossibleResult(string[] results)
        {
            var possibleResult = Utils.GetClosestToEnglish(results);
            Console.WriteLine(possibleResult);
            File.WriteAllText(GetPathInProject("results/task2-possible.txt"), possibleResult);
        }

        private static byte[] Xor(byte[] bytes, byte k)
        {
            return bytes.Select(b => (byte)(b ^ k)).ToArray();
        }

        private static void FromBinaryToText()
        {
            var text = File.ReadAllText(GetPathInProject("data/task1.txt"));
            var bytes = StringEncoder.GetBytes(text);
            var decoded = StringEncoder.GetString(bytes);
            var baseString = StringEncoder.GetBytes(decoded);
            File.WriteAllBytes(GetPathInProject("results/task1.txt"), baseString);
        }

        private static string GetPathInProject(string path)
        {
            return $"../../../{path}";
        }
    }
}
