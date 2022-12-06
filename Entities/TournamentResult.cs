using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class TournamentResult
    {
        public TournamentResult(List<MatchResult> matchResults, DateTime nowUtc, GameType gameType, VersusMode versusMode)
        {
            this.MatchResults = matchResults;
            this.TournamentHeldAt = nowUtc;
            this.GameType = gameType;
            this.VersusMode = versusMode;
            this.PlayerList = matchResults.SelectMany(m => m.Players).Distinct().ToList();
        }

        public List<MatchResult> MatchResults { get; }

        public DateTime TournamentHeldAt { get; }

        public GameType GameType { get; }

        public VersusMode VersusMode { get; }

        /// <summary>
        /// Gets the list of all players participating in the tournament.
        /// </summary>
        public List<Player> PlayerList { get; }

        public string TournamentOverviewDescription()
        {
            return $"Tournament for gametype: {this.GameType} with VersusMode: {this.VersusMode} held at: {this.TournamentHeldAt} with {this.MatchResults.Count} matches";
        }

        public string GetTournamentResultsString()
        {
            return string.Empty;
        }
    }
}
