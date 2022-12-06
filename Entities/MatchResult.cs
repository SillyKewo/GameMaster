using System.Text;

namespace Entities
{
    public class MatchResult
    {
        private Dictionary<Player, int> playerWins;
        private int countDraw;
        private int countTimedOut;

        public MatchResult(List<GameResult> gameResults, GameBoardConfiguration gameBoardConfiguration)
        {
            this.GameResults = gameResults;
            this.Players = gameResults.SelectMany(g => g.Players).Distinct().ToList();

            Dictionary<Player, int> playerWins = this.GameResults.SelectMany(x => x.Players).Distinct().ToDictionary(p => p, _ => 0);
            int countDraw = 0;
            int countTimedOut = 0;
            foreach (GameResult gameResult in this.GameResults)
            {
                switch (gameResult.GameResultCondition)
                {
                    case GameResult.ResultCondition.Normal:
                        if (gameResult.HasWinner && gameResult.ResultConditionPlayer is not null)
                        {
                            playerWins[gameResult.ResultConditionPlayer]++;
                        }
                        break;
                    case GameResult.ResultCondition.Draw:
                        countDraw++;
                        break;
                    case GameResult.ResultCondition.TimeOut:
                        countTimedOut++;
                        break;
                }
            }

            this.playerWins = playerWins;
            this.countDraw = countDraw;
            this.countTimedOut = countTimedOut;

            int maxWins = playerWins.Values.Max();
            List<Player> playersWithMaxWins = new List<Player>();

            foreach (KeyValuePair<Player, int> item in playerWins)
            {
                if (item.Value == maxWins)
                {
                    playersWithMaxWins.Add(item.Key);
                }
            }

            if (playersWithMaxWins.Count == 1)
            {
                this.Winner = playersWithMaxWins[0];
            }

            this.IsDraw = playersWithMaxWins.Count > 1;
            this.GameBoardConfiguration = gameBoardConfiguration;
        }

        public List<GameResult> GameResults { get; }

        public List<Player> Players { get; }

        public Player? Winner { get; }

        public bool IsDraw { get; }

        public GameBoardConfiguration GameBoardConfiguration { get; }

        public string MatchResultDescription()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Match results:");
            sb.AppendLine($"Number of games played: {this.GameResults.Count}");
            sb.AppendLine();

            sb.AppendLine($"Games ending in draw: {countDraw}");
            sb.AppendLine($"Games ending in timed out: {countTimedOut}");

            sb.AppendLine();
            sb.AppendLine($"Player wins:");

            foreach (KeyValuePair<Player, int> playerAndWins in playerWins.OrderBy(kv => kv.Value))
            {
                sb.AppendLine($"Player: {playerAndWins.Key.Name} with {playerAndWins.Value} win(s)");
            }
            sb.AppendLine("-----------------------------------------------------");
            sb.AppendLine("Game Descriptions!");

            for (int i = 0; i < this.GameResults.Count; i++)
            {
                sb.AppendLine($"Game {i}:");
                sb.AppendLine(this.GameResults[i].ResultDescription());
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
