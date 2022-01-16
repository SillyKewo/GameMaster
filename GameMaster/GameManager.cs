using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    public class GameManager
    {

        public GameManager(IGame game)
        {
            this.Game = game;
        }

        public IGame Game { get; }

        public void Play()
        {
            this.Game.Initialize();

            while (!this.Game.IsDone)
            {
                IGamePlayer nextPlayer = this.Game.NextPlayer();

                Move move = nextPlayer.NextMove(this.Game.GetGameState());

                this.Game.PlaceMove(nextPlayer, move);
            }

        }

        public bool IsDone => this.Game.IsDone;

        public string GetResult()
        {
            IGamePlayer? gamePlayer = this.Game.GetWinner();

            if (gamePlayer is null)
            {
                return "No winner";
            }
            else
            {
                return $"Winner is {gamePlayer.User}";
            }
        }

    }
}
