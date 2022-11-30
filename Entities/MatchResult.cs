using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class MatchResult
    {

        public MatchResult(List<GameResult> gameResults)
        {
            this.GameResults = gameResults;
        }

        public List<GameResult> GameResults { get; }

        public string MatchResultDescription()
        {
            StringBuilder sb = new StringBuilder();
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
