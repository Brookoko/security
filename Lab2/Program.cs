namespace Lab2
{
    using System;
    using System.IO;
    using System.Linq;
    using security;

    class Program
    {
        private static readonly StringEncoder StringEncoder = new();

        private const string TestString =
            "280dc9e47f3352c307f6d894ee8d534313429a79c1d8a6021f8a8eabca919cfb685a0d468973625e757490daa981ea6b";

        private static void Main(string[] args)
        {
            var lines = ReadAllLines();
            for (var i = 0; i < lines.Length; i++)
            {
                TestWithKey(i, lines[i], "For who would bear the ships and scorns of time,");
            }
        }

        private static string[] ReadAllLines()
        {
            var text = File.ReadAllText(GetPathInProject("data.txt"));
            return text.Split('\n').Select(s => s.Replace("\r", "")).ToArray();
        }

        private static void TestWithKey(int index, string line, string text)
        {
            var b1 = StringEncoder.GetBytes(TestString);
            var b2 = StringEncoder.GetBytes(line);
            var msg = Xor(b1, b2);
            var bk = StringEncoder.GetBytes(text);
            var res = Xor(msg, bk);
            Print(index, res);
        }

        private static byte[] Xor(byte[] b1, byte[] b2)
        {
            var length = Math.Min(b1.Length, b2.Length);
            var res = new byte[length];
            for (var i = 0; i < length; i++)
            {
                res[i] = (byte)(b1[i] ^ b2[i]);
            }
            return res;
        }

        private static void Print(int index, byte[] b)
        {
            Console.WriteLine($"{index + 1} - {StringEncoder.GetString(b)}");
        }

        private static string GetPathInProject(string path)
        {
            return $"../../../{path}";
        }
    }
}
