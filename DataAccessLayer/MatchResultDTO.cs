using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster.DataAccessLayer
{
    public class MatchResultDTO
    {
        public List<string> Players { get; set; }

        public List<GameResultDTO> GameResults { get; set; }
    }
}
