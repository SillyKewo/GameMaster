using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster
{
    /// <summary>
    /// The class responsible for simulating a single game and report results.
    /// </summary>
    public class GameManager
    {
        private readonly int _timeOutSec;

        /// <summary>
        /// Initializes the <see cref="GameManager"/> instance
        /// </summary>
        /// <param name="game">The game to be managed</param>
        /// <param name="timeOutSec">Move time out.</param>
        public GameManager(IGame game, int timeOutSec)
        {
            this.Game = game;
            this._timeOutSec = timeOutSec;
        }

        /// <summary>
        /// Gets the game managed by the instance
        /// </summary>
        public IGame Game { get; }

        /// <summary>
        /// Plays out the game.
        /// </summary>
        /// <returns>Returns a <see cref="GameResult"/> instance</returns>
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

        /// <summary>
        /// Gets a value indicating whether the game has been played to completion.
        /// </summary>
        public bool IsDone => this.Game.IsDone || this.TimedOutPlayer != null;

        /// <summary>
        /// Gets the possibly null player that timed out, if any.
        /// </summary>
        public Player? TimedOutPlayer { get; private set; }  

        /// <summary>
        /// Gets the result as a string representation.
        /// </summary>
        /// <returns></returns>
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
