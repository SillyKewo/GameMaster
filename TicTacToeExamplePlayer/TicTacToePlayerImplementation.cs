using GamePlayerInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeExamplePlayer
{
    public class TicTacToePlayerImplementation : ITicTacToePlayer
    {
        public TicTacToePlayerImplementation()
        {

        }


        public void Initialize(bool isStarting)
        {
        }

        public List<int> NextMove(int[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == 0)
                    {
                        return new List<int> { i, j };
                    }
                }
            }

            return new List<int> { 0, 0 };
        }
    }
}
