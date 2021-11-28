namespace Lab3
{
    using System;
    using System.Threading.Tasks;
    using Data;

    public class Program
    {
        private static readonly NetworkWorker NetworkWorker = new();

        private static async Task Main(string[] args)
        {
            await PlayLcg();
        }

        private static async Task PlayLcg()
        {
            var account = await CreateNewAccount();
            var result = await NetworkWorker.Play(account, 1, 1, Mode.Lcg);
            Console.WriteLine(result);
        }

        private static async Task<Account> CreateNewAccount()
        {
            var random = new Random();
            var id = random.Next();
            return await NetworkWorker.CreateNewAccount(id);
        }
    }
}
