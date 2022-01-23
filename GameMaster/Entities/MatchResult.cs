using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster.Entities
{
    public class MatchResult
    {

        public MatchResult(List<GameResult> gameResults)
        {
            this.GameResults = gameResults;
        }

        public List<GameResult> GameResults { get; }
    }
}
