using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class TournamentResult
    {
        public TournamentResult(List<MatchResult> matchResults, DateTime nowUtc, GameType gameType)
        {
            this.MatchResults = matchResults;
            this.TournamentHeldAt = nowUtc;
            this.GameType = gameType;
        }

        public List<MatchResult> MatchResults { get; }

        public DateTime TournamentHeldAt { get; }

        public GameType GameType { get; }
    }
}
