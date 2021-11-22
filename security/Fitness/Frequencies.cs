namespace security
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class Frequencies
    {
        public static readonly Dictionary<string, double> UnigramsFrequency = new();
        public static readonly Dictionary<string, double> BigramsFrequency = new();
        public static readonly Dictionary<string, double> TriramsFrequency = new();

        public static readonly double UniramsFloor = 0;
        public static readonly double BiramsFloor = 0;
        public static readonly double TriramsFloor = 0;

        static Frequencies()
        {
            UniramsFloor = PopulateTxt(UnigramsFrequency, "grams/1grams.txt");
            BiramsFloor = PopulateTxt(BigramsFrequency, "grams/2grams.txt");
            TriramsFloor = PopulateTxt(TriramsFrequency, "grams/3grams.txt");
            // PopulateCsv(MonogramsFrequency, "grams/1grams.csv");
            // PopulateCsv(BigramsFrequency, "grams/2grams.csv");
            // PopulateCsv(TriramsFrequency, "grams/3grams.csv");
        }

        private static double PopulateTxt(Dictionary<string, double> grams, string file)
        {
            file = GetPathInProject(file);
            using var reader = new StreamReader(file);
            decimal sum = 0;
            var counts = new Dictionary<string, decimal>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var fields = line.Split(' ');
                var gram = fields[0];
                var amount = decimal.Parse(fields[1]);
                counts[gram] = amount;
                sum += amount;
            }
            foreach (var (gram, amount) in counts)
            {
                grams[gram] = Math.Log10(decimal.ToDouble(decimal.Divide(amount, sum)));
            }
            return Math.Log10(decimal.ToDouble(decimal.Divide((decimal)0.01, sum)));
        }

        private static void PopulateCsv(Dictionary<string, double> grams, string file)
        {
            file = GetPathInProject(file);
            using var reader = new StreamReader(file);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var fields = line.Split(',');
                var gram = fields[0];
                var freq = double.Parse(fields[2]);
                grams[gram] = freq;
            }
        }

        private static string GetPathInProject(string path)
        {
            return $"../../../{path}";
        }
    }
}
