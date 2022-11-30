using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster.DataAccessLayer
{
    public class GameResultDTO
    {
        public List<MoveDTO> Moves { get; set; }

        public string? ConditionPlayer { get; set; }

        public List<string> Players { get; set; }

        public bool TimedOut { get; set; }

        public bool HasWinner { get; set; }

        public List<float>? Scores { get; set; }
    }
}
