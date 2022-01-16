using GamePlayerInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    public class TicTacToePlayerWrapper : IGamePlayer
    {
        private ITicTacToePlayer _player;
        private readonly string _userString;

        public TicTacToePlayerWrapper(object player, string user)
        {
            this._player = player as ITicTacToePlayer;
            this._userString = user;

            if (this._player is null)
            {
                throw new NullReferenceException();
            }
        }

        public string User => this._userString;

        public void Initialize(bool isStarting)
        {
            this._player.Initialize(isStarting);
        }

        public Move NextMove(IGameState gameState)
        {
            var move = this._player.NextMove((int[,])gameState.InternalBoardState);
            return new Move(move);
        }
    }
}
