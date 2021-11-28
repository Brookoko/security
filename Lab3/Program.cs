namespace Lab3
{
    using System;
    using System.Threading.Tasks;
    using Cracker;
    using Data;
    using Generators;
    using PseudoRandom;

    public class Program
    {
        private static readonly NetworkWorker NetworkWorker = new();
        private const long TargetMoney = 1_000_000;
        private const long BetMoney = 100;

        private static async Task Main(string[] args)
        {
            // await PlayLcg();
            await PlayMt();
        }

        private static async Task PlayLcg()
        {
            var account = await CreateNewAccount();

            var sequence = new long[3];
            for (var i = 0; i < 3; i++)
            {
                var result = await NetworkWorker.Play(account, 1, 1, Mode.Lcg);
                sequence[i] = result.RealNumber;
            }

            var lcgCracker = new LcgCracker();
            var (a, b) = lcgCracker.CrackLcg(sequence);
            var lcg = new Lcg(a, b, sequence[2]);
            Console.WriteLine($"Found constant for lcg: a = {a} and b = {b}");

            while (account.Money < TargetMoney)
            {
                var result = await NetworkWorker.Play(account, BetMoney, lcg.Next(), Mode.Lcg);
                Console.WriteLine(result);
            }
        }

        private static async Task PlayMt()
        {
            var account = await CreateNewAccount();

            var seed = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 1;
            var mt = new MersenneTwister();
            mt.init_genrand((ulong)seed);
            var playedGames = 0;
            Console.WriteLine($"Seed: {seed}");

            while (account.Money < TargetMoney)
            {
                var result = await NetworkWorker.Play(account, BetMoney, (uint)mt.genrand_uint32(), Mode.Mt);
                Console.WriteLine(result);
                playedGames++;
                if (!result.IsWon)
                {
                    Console.WriteLine($"Seed is wrong. Try to correct seed");
                    seed -= 1;
                    mt.init_genrand((ulong)seed);
                    for (var i = 0; i < playedGames; i++)
                    {
                        mt.genrand_uint32();
                    }
                }
            }
        }

        private static async Task<Account> CreateNewAccount()
        {
            var random = new Random();
            var id = random.Next();
            return await NetworkWorker.CreateNewAccount(id);
        }
    }
}
