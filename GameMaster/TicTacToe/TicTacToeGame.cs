using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    class TicTacToeGame : IGame
    {
        private IGamePlayer _player1;
        private IGamePlayer _player2;

        private int currentPlayer = 0;
        private List<IGamePlayer> _players;
        private readonly int _initialPlayerIndex;
        private BoardState? _gameState;


        private TicTacToeGame(IGamePlayer player1, IGamePlayer player2, int initialPlayer)
        {
            this._player1 = player1;
            this._player2 = player2;
            this._players = new List<IGamePlayer>() { player1, player2 };
            this._initialPlayerIndex = initialPlayer;
            this.currentPlayer = initialPlayer;
        }


        public string Description => $"Tic-Tac-Toe game: Player1 ({this._player1.Player.Name}) vs Player2 ({this._player2.Player.Name})";

        public bool IsDone { get
            {
                if (this._gameState is null)
                    throw new InvalidOperationException($"Game has not been initialized yet!");

                return this._gameState.IsDone;
            } }

        public List<IGamePlayer> Players => this._players;

        public IGamePlayer? GetWinner()
        {
            if (this._gameState is null)
            {
                throw new InvalidOperationException($"Game has not been initialized yet!");
            }

            int winner = this._gameState.Winner;

            return winner switch
            {
                0 => null,
                1 => this._player1,
                2 => this._player2,
                _ => throw new InvalidOperationException()
            };
        }

        public void Initialize()
        {
            this._gameState = BoardState.Create();
            this._player1.Initialize(this._initialPlayerIndex == 0);
            this._player2.Initialize(this._initialPlayerIndex == 1);
        }

        public IGameState GetGameState()
        {
            if (this._gameState is null)
            {
                throw new InvalidOperationException($"Game has not been initialized yet!");
            }

            return this._gameState;
        }

        public void PlaceMove(IGamePlayer gamePlayer, Move move)
        {
            if (this._gameState is null)
            {
                throw new InvalidOperationException($"Game has not been initialized yet!");
            }

            if (move.Commands.Count > 1)
            {
                if (gamePlayer.Player == this._player1.Player)
                {
                    this._gameState.PlaceMove(((int)move.Commands[0], (int)move.Commands[1]), 1);
                }
                else
                {
                    this._gameState.PlaceMove(((int)move.Commands[0], (int)move.Commands[1]), 2);
                }
            }
            else
            {
                throw new InvalidOperationException($"Unexpected move count: {move.Commands.Count}");
            }

            this.currentPlayer = (this.currentPlayer + 1) % 2;
        }

        public IGamePlayer NextPlayer()
        {
            IGamePlayer currentPlayer = this.Players[this.currentPlayer];
            return currentPlayer;
        }

        public static TicTacToeGame Create(List<IGamePlayer> players, int startPlayerIndex)
        {
            if (players.Count != 2)
            {
                throw new InvalidOperationException($"{typeof(TicTacToeGame).Name} requires exactly 2 players!");
            }

            return new TicTacToeGame(players[0], players[1], startPlayerIndex);
        }

        public class BoardState : IGameState
        {
            private int[,] _board;
            private int _freeSpots;
            private int _winner;

            private const int BoardSize = 3;

            private BoardState(int[,] board)
            {
                this._board = board;
                this._freeSpots = board.Length;
            }

            public int[,] Board => this._board;

            public static BoardState Create()
            {
                int[,] board = new int[BoardSize, BoardSize];

                return new BoardState(board);
            }

            public bool PlaceMove((int Row, int Column) move, int player)
            {
                if (this._board[move.Row, move.Column] != 0)
                {
                    return false;
                }

                this._board[move.Row, move.Column] = player;

                this._freeSpots--;

                // check rows
                if (this._board[0, 0] == player && this._board[0, 1] == player && this._board[0, 2] == player) { this._winner = player; return true; }
                if (this._board[1, 0] == player && this._board[1, 1] == player && this._board[1, 2] == player) { this._winner = player; return true; }
                if (this._board[2, 0] == player && this._board[2, 1] == player && this._board[2, 2] == player) { this._winner = player; return true; }

                // check columns
                if (this._board[0, 0] == player && this._board[1, 0] == player && this._board[2, 0] == player) { this._winner = player; return true; }
                if (this._board[0, 1] == player && this._board[1, 1] == player && this._board[2, 1] == player) { this._winner = player; return true; }
                if (this._board[0, 2] == player && this._board[1, 2] == player && this._board[2, 2] == player) { this._winner = player; return true; }

                // check diags
                if (this._board[0, 0] == player && this._board[1, 1] == player && this._board[2, 2] == player) { this._winner = player; return true; }
                if (this._board[0, 2] == player && this._board[1, 1] == player && this._board[2, 0] == player) { this._winner = player; return true; }

                return true;
            }

            public bool IsDone => this._freeSpots == 0 || this._winner != 0;

            public int Winner => this._winner;

            public GameType GameType => GameType.TicTacToe;

            public object InternalBoardState => this._board;
        }
    }
}
