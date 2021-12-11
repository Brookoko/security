namespace Lab4
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Security.Server.Hashing;

    public class Program
    {
        private const int NumberOfThread = 16;
        private static object locker = new();

        private static void Main(string[] args)
        {
            TryBreakArgon();
        }

        private static void TryBreakArgon()
        {
            var argon = new Argon2i(4096 * 1024, 3);
            var passwords = ReadPasswords();
            var hashes = ReadHashes();
            var foundPasswords = new Dictionary<string, string>();
            var threads = new Thread[NumberOfThread];
            var numberOfPasswords = passwords.Length / NumberOfThread;
            var extra = passwords.Length % NumberOfThread;
            for (var i = 0; i < NumberOfThread; i++)
            {
                var number = i < extra ? numberOfPasswords + 1 : numberOfPasswords;
                var skippedPasswords = i * numberOfPasswords + (extra - Math.Max(0, extra - i));
                var threadPasswords = passwords.Skip(skippedPasswords).Take(number);
                threads[i] = new Thread(() => ProcessPasswords(argon, threadPasswords, hashes, foundPasswords));
                threads[i].Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            var builder = new StringBuilder();
            foreach (var (h, p) in foundPasswords)
            {
                builder.Append(h);
                builder.Append(" -> ");
                builder.Append(p);
            }

            Console.WriteLine($"Found: {foundPasswords.Count}");
            File.WriteAllText(GetPathInProject("data/strong_cracked.txt"), builder.ToString());
        }

        private static void ProcessPasswords(Argon2i argon, IEnumerable<string> passwords, string[] hashes,
            Dictionary<string, string> foundPasswords)
        {
            foreach (var password in passwords)
            {
                Console.WriteLine($"Processing {password}");
                foreach (var hash in hashes)
                {
                    if (argon.Verify(password, hash))
                    {
                        Console.WriteLine($"Found password: {hash} -> {password}");
                        foundPasswords[hash] = password;
                        Save(hash, password);
                    }
                }
            }
        }

        private static void Save(string hash, string password)
        {
            lock (locker)
            {
                using var w = File.AppendText(GetPathInProject("data/strong_cracked.txt"));
                w.WriteLine($"{hash} -> {password}");
                w.Flush();
            }
        }

        private static string[] ReadPasswords()
        {
            return File.ReadAllLines(GetPathInProject("data/common_passwords.txt"));
        }

        private static string[] ReadHashes()
        {
            return File.ReadAllLines(GetPathInProject("data/strong_hashes.txt"));
        }

        private static string GetPathInProject(string path)
        {
            return $"../../../{path}";
        }
    }
}
