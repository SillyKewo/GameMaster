using GamePlayerInterfaces;
using System;

namespace GameMaster
{
    /// <summary>
    /// The collection of game types supported.
    /// </summary>
    public enum GameType
    {
        TicTacToe = 0,
        RockPaperScissors = 1,
        DodgeBall = 2,
    }

    /// <summary>
    /// Extension methods for <see cref="GameType"/>
    /// </summary>
    public static class GameTypeExtensions
    {
        /// <summary>
        /// Gets the associated player type for the <see cref="GameType"/>
        /// </summary>
        /// <param name="o">this game type.</param>
        /// <returns>The type of the player.</returns>
        public static Type GetPlayerType(this GameType o)
        {
            return o switch
            {
                GameType.TicTacToe => typeof(ITicTacToePlayer),
                GameType.DodgeBall => typeof(IDodgeBallPlayer),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
