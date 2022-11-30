using GameMaster.Entities;
using GamePlayerInterfaces;
using GamePlayerInterfaces.DodgeBall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster.Dodgeball
{
    public class DodgeBallPlayerWrapper : IGamePlayer
    {
        private readonly IDodgeBallPlayer _playerImplementation;

        public DodgeBallPlayerWrapper(object playerImplementation, Player player)
        {
            if (!(playerImplementation is IDodgeBallPlayer playerImplemantationCast))
            {
                throw new NullReferenceException();
            }
            this._playerImplementation = playerImplemantationCast;
            this.Player = player;
        }

        public Player Player { get; }

        public void Initialize(bool isStarting)
        {
            throw new NotImplementedException();
        }

        public Move NextMove(IGameState gameState)
        {
            var internalGameState = (IDodgeBallGameState)gameState.InternalBoardState;
            return new Move(new List<float>(), this.Player);
        }

        
    }
}
