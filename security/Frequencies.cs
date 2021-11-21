namespace security
{
    using System.Collections.Generic;
    using System.IO;

    public class Frequencies
    {
        public static readonly Dictionary<string, double> BigramsFrequency = new();
        public static readonly Dictionary<string, double> TriramsFrequency = new();

        public static readonly Dictionary<char, double> SingleLetterFrequency = new()
        {
            { 'A', 0.082 },
            { 'B', 0.015 },
            { 'C', 0.028 },
            { 'D', 0.043 },
            { 'E', 0.13 },
            { 'F', 0.022 },
            { 'G', 0.02 },
            { 'H', 0.061 },
            { 'I', 0.070 },
            { 'J', 0.0015 },
            { 'K', 0.0077 },
            { 'L', 0.04 },
            { 'M', 0.024 },
            { 'N', 0.067 },
            { 'O', 0.075 },
            { 'P', 0.019 },
            { 'Q', 0.00095 },
            { 'R', 0.06 },
            { 'S', 0.063 },
            { 'T', 0.091 },
            { 'U', 0.028 },
            { 'V', 0.0098 },
            { 'W', 0.024 },
            { 'X', 0.0015 },
            { 'Y', 0.02 },
            { 'Z', 0.00074 },
        };

        static Frequencies()
        {
            Populate(BigramsFrequency, "2grams.csv");
            Populate(TriramsFrequency, "3grams.csv");
        }

        private static void Populate(Dictionary<string, double> grams, string file)
        {
            file = GetPathInProject(file);
            using var reader = new StreamReader(file);
            var count = 0;
            while (count < 200 && !reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var fields = line.Split(',');
                var gram = fields[0];
                var freq = double.Parse(fields[2]);
                grams[gram] = freq;
                count++;
            }
        }

        private static string GetPathInProject(string path)
        {
            return $"../../../{path}";
        }
    }
}
