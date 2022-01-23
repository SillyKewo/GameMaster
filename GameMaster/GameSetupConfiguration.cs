using System.Collections.Generic;

namespace GameMaster
{
    public class GameSetupConfiguration
    {
        public GameType GameType { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string PlayerFolder { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public VersusMode VersusMode { get; set; }

        public int RoundsPerMatch { get; set; }

        public int TimeOutSec { get; set; }
    }
}
