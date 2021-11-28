namespace Lab3
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Data;
    using Newtonsoft.Json;

    public class NetworkWorker
    {
        private const string Url = "http://95.217.177.249/casino";

        private readonly HttpClient client = new HttpClient();

        public async Task<Account> CreateNewAccount(int id)
        {
            var url = $"{Url}/createacc?id={id}";
            return await GetAsync<Account>(url);
        }

        public async Task<GameResult> Play(Account account, int bet, long number, Mode mode)
        {
            var url = $"{Url}/play{mode}?id={account.Id}&bet={bet}&number={number}";
            var gameResult = await GetAsync<GameResult>(url);
            gameResult.Bet = bet;
            gameResult.BetNumber = number;
            return gameResult;
        }

        private async Task<T> GetAsync<T>(string url)
        {
            var response = await client.GetAsync(url);
            var message = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(message);
        }
    }
}
