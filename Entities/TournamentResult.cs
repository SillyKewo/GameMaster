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
        }

        public List<MatchResult> MatchResults { get; }

        public DateTime TournamentHeldAt { get; }

        public GameType GameType { get; }

        public VersusMode VersusMode { get; }

        public string TournamentOverviewDescription()
        {
            return $"Tournament for gametype:{this.GameType} with VersusMode:{this.VersusMode} held at:{this.TournamentHeldAt} with {this.MatchResults.Count} matches";
        }

        public string GetTournamentResultsString()
        {
            return string.Empty;
        }

    }
}
