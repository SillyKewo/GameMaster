using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    public interface IGameState
    {
        public GameType GameType { get; }

        public object InternalBoardState { get; }
    }
}
