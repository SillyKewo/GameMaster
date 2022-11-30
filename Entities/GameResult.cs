using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class GameResult
    {
        public enum ResultCondition
        {
            Normal,
            Draw,
            TimeOut
        }

        private GameResult(List<Player> players, List<Move> moves, ResultCondition condition, Player? resultPlayer)
        {
            if (condition == ResultCondition.Draw && resultPlayer != null)
            {
                throw new InvalidOperationException($"When result is draw, then resultPlayer should be null");
            }

            if (condition != ResultCondition.Draw && resultPlayer == null)
            {
                throw new ArgumentException($"result player can not be null");
            }

            this.Players = players;
            this.Moves = moves;
            this.GameResultCondition = condition;
            this.ResultConditionPlayer = resultPlayer;
        }

        public List<Player> Players { get; }

        public List<Move> Moves { get; }

        public ResultCondition GameResultCondition { get; }

        public bool HasWinner => this.GameResultCondition == ResultCondition.Normal;

        public Player? ResultConditionPlayer { get; }

        public static GameResult CreateDrawResult(List<Player> players, List<Move> moves)
        {
            return new GameResult(players, moves, ResultCondition.Draw, null);
        }

        public static GameResult CreateResult(List<Player> players, List<Move> moves, Player winner)
        {
            return new GameResult(players, moves, ResultCondition.Normal, winner);
        }

        public static GameResult CreateTimedOutResult(List<Player> players, List<Move> moves, Player timedOut)
        {
            return new GameResult(players, moves, ResultCondition.TimeOut, timedOut);
        }

        public string ResultDescription()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Game between: {string.Join(',', this.Players.Select(p => p.Name))}.");
            sb.AppendLine($"Game end was: {this.GameResultCondition}");
            if (this.GameResultCondition != ResultCondition.Draw)
            {
                sb.AppendLine($"{(this.GameResultCondition == ResultCondition.Normal ? "Winner" : "Time out player")} was {this.ResultConditionPlayer!.Name}");
            }

            sb.AppendLine();
            sb.AppendLine($"Total number of moves: {this.Moves.Count}");

            return sb.ToString();
        }
    }
}
