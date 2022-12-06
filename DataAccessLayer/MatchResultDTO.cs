using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameMaster.DataAccessLayer
{
    public class MatchResultDTO
    {
        public List<string> Players { get; set; }

        public List<GameResultDTO> GameResults { get; set; }

        [XmlElement(typeof(TicTacToeGameBoardDTO), ElementName = "TicTacToeGameBoardDTO")]
        public GameBoardConfigDTO GameBoardConfig { get; set; }
    }

    [XmlInclude(typeof(TicTacToeGameBoardDTO))]
    public class GameBoardConfigDTO
    {

    }

    public class TicTacToeGameBoardDTO : GameBoardConfigDTO
    {
        public int BoardSize { get; set;}
    }
}
