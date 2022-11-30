using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster.DataAccessLayer
{
    public class TournamentResultDTO
    {
        public List<MatchResultDTO> MatchResults { get; set; }

        public DateTime DateTime { get; set; }

        public GameType GameType { get; set; }
    }
}
