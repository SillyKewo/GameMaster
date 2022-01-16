using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    public interface IGame
    {
        public string Description { get; }

        public bool IsDone { get; }

        public void Initialize();

        public List<IGamePlayer> Players { get; }

        public IGamePlayer? GetWinner();

        public IGameState GetGameState();

        public void PlaceMove(IGamePlayer gamePlayer, Move move);

        public IGamePlayer NextPlayer();
    }
}
