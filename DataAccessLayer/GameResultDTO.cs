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

        public string? Player { get; set; }

        public bool TimedOut { get; set; }

        public bool HasWinner { get; set; }
    }
}
