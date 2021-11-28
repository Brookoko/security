namespace Lab3.Data
{
    public class GameResult
    {
        public string Message { get; set; }

        public Account Account { get; set; }

        public long RealNumber { get; set; }

        public long Bet { get; set; }

        public long BetNumber { get; set; }

        public override string ToString()
        {
            return $"Game result: {Message}\nBet: {Bet}\nBet number: {BetNumber}\nReal number: {RealNumber}";
        }
    }
}
