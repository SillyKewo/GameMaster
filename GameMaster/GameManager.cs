using GameMaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster
{
    public class GameManager
    {
        private readonly int _timeOutSec;
        public GameManager(IGame game, int timeOutSec)
        {
            this.Game = game;
            this._timeOutSec = timeOutSec;
        }

        public IGame Game { get; }

        public GameResult Play()
        {
            this.Game.Initialize();
            List<Move> moves = new List<Move>();
            while (!this.Game.IsDone)
            {
                IGamePlayer nextPlayer = this.Game.NextPlayer();

                Task<Move> playerTask = Task.Run(() => nextPlayer.NextMove(this.Game.GetGameState()));
                Move move;
                if (playerTask.Wait(TimeSpan.FromSeconds(this._timeOutSec)))
                {
                    move = playerTask.Result;
                    moves.Add(move);
                    this.Game.PlaceMove(nextPlayer, move);
                }
                else
                {
                    this.TimedOutPlayer = nextPlayer.Player;
                    break;
                }

            }

            if (this.TimedOutPlayer != null)
            {
                return GameResult.CreateTimedOutResult(this.Game.Players.Select(gp => gp.Player).ToList(), moves, this.TimedOutPlayer);
            }

            IGamePlayer? winner = this.Game.GetWinner();
            if (winner == null) 
            {
                return GameResult.CreateDrawResult(this.Game.Players.Select(gp => gp.Player).ToList(), moves);
            }
            else
            {
                return GameResult.CreateResult(this.Game.Players.Select(gp => gp.Player).ToList(), moves, winner.Player);
            }

        }

        public bool IsDone => this.Game.IsDone || this.TimedOutPlayer != null;

        public Player? TimedOutPlayer { get; private set; }  

        public string GetResult()
        {
            IGamePlayer? gamePlayer = this.Game.GetWinner();

            if (gamePlayer is null)
            {
                return this.TimedOutPlayer == null ? "Draw" : $"Player: {this.TimedOutPlayer.Name}, timed out! time limit is {this._timeOutSec}sec";
            }
            else
            {
                return $"Winner is {gamePlayer.Player.Name}";
            }
        }

    }
}
