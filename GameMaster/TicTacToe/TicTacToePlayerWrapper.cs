using GameMaster.Entities;
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
        private ITicTacToePlayer _playerImplementation;
        private readonly Player _player;

        public TicTacToePlayerWrapper(object playerImplementation, Player player)
        {
            if (!(playerImplementation is ITicTacToePlayer playerImplemantationCast))
            {
                throw new NullReferenceException();
            }
            this._playerImplementation = playerImplemantationCast;
            this._player = player;
        }

        public Player Player => this._player;

        public void Initialize(bool isStarting)
        {
            this._playerImplementation.Initialize(isStarting);
        }

        public Move NextMove(IGameState gameState)
        {
            var move = this._playerImplementation.NextMove((int[,])gameState.InternalBoardState);
            return new Move(move, this._player);
        }
    }
}
