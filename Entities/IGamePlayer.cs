using System.Diagnostics;

namespace Entities
{
    /// <summary>
    /// A player representation that can place moves.
    /// </summary>
    public interface IGamePlayer
    {
        /// <summary>
        /// Gets the player instance
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Ask the player for its next move.
        /// </summary>
        /// <param name="gameState">The current game state.</param>
        /// <returns>The player move</returns>
        public Move NextMove(IGameState gameState);

        /// <summary>
        /// Initialize the player instance 
        /// </summary>
        /// <param name="isStarting">Whether the player is starting.</param>
        public void Initialize(bool isStarting);
    }
}
