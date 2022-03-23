using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    /// <summary>
    /// A game state.
    /// </summary>
    public interface IGameState
    {
        /// <summary>
        /// The game type of the game state.
        /// </summary>
        public GameType GameType { get; }

        /// <summary>
        /// Gets an internal board state.
        /// </summary>
        public object InternalBoardState { get; }
    }
}
