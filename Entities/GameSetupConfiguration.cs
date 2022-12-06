
using System.Xml.Serialization;

namespace Entities
{
    public class GameSetupConfiguration
    {
        public GameType GameType { get; set; }

        [XmlElement(typeof(TicTacToeGameBoardConfiguration), ElementName = "TicTacToeGameBoardConfiguration")]
        public GameBoardConfiguration GameBoardConfiguration { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string PlayerFolder { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public VersusMode VersusMode { get; set; }

        public int RoundsPerMatch { get; set; }

        public int TimeOutSec { get; set; }

        public int PlayersPerGame { get; set; }
    }

    [XmlInclude(typeof(TicTacToeGameBoardConfiguration))]
    public class GameBoardConfiguration
    {

    }

    public class TicTacToeGameBoardConfiguration : GameBoardConfiguration
    {
        public int BoardSize { get; set; }
    }
}
