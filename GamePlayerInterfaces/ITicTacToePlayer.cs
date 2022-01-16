using System;
using System.Collections.Generic;

namespace GamePlayerInterfaces
{
    public interface ITicTacToePlayer
    {
        public void Initialize(bool isStarting);

        public List<int> NextMove(int[,] board);

    }
}
