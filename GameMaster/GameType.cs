using GamePlayerInterfaces;
using System;

namespace GameMaster
{
    public enum GameType
    {
        TicTacToe = 0,
        RockPaperScissors = 1 
    }

    public static class GameTypeExtensions
    {
        public static Type GetPlayerType(this GameType o)
        {
            return o switch
            {
                GameType.TicTacToe => typeof(ITicTacToePlayer),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
