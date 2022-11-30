using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    /// <summary>
    /// A game that can be played between a collection of <see cref="IGamePlayer"/>
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Gets the description of the game.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets a value indicating whether the game is played to completion, e.g. is done.
        /// If done no further moves can be applied.
        /// </summary>
        public bool IsDone { get; }

        /// <summary>
        /// Initializes the game, should be called before placing moves.
        /// </summary>
        public void Initialize();

        /// <summary>
        /// Collection of players in the game.
        /// </summary>
        public List<IGamePlayer> Players { get; }

        /// <summary>
        /// Retrieves the winner of the game, if any at the current state.
        /// </summary>
        /// <returns>The possibly null, winner of the game.</returns>
        public IGamePlayer? GetWinner();

        /// <summary>
        /// Gets the current game state.
        /// </summary>
        /// <returns></returns>
        public IGameState GetGameState();

        /// <summary>
        /// Place a move for a give <see cref="IGamePlayer"/>.
        /// </summary>
        /// <param name="gamePlayer">The player that places the move.</param>
        /// <param name="move">The move to be placed.</param>
        public void PlaceMove(IGamePlayer gamePlayer, Move move);

        /// <summary>
        /// Gets the next player to place a move.
        /// </summary>
        /// <returns>The <see cref="IGamePlayer"/> to place a move.</returns>
        public IGamePlayer NextPlayer();
    }
}
