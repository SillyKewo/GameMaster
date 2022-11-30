using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Entities;
using GamePlayerInterfaces.DodgeBall;

namespace GameMaster
{
    public class DodgeBallGame : IGame
    {
        private List<(IGamePlayer Player, Vector2 Position)> _playerPositions;
        private Dictionary<IGamePlayer, DodgeBall> _playerDodgeBalls;
        private Dictionary<DodgeBall, Vector2> _dodgeBallPositions;
        private int _currentPlayerIndex = 0;
        private const float AreaSize = 100.0f;
        private const int MaxMoves = 2000;
        private DodgeBallGameState _currentGameState;
        private int _moveCounter = 0;
        private List<IGamePlayer> _players;

        public DodgeBallGame(List<IGamePlayer> gamePlayers, int startPlayerIndex)
        {
            if (gamePlayers.Count > 4)
            {
                throw new ArgumentException($"Can only handle up to 4 players, game created with {gamePlayers.Count} players");
            }
            this._players = gamePlayers;
            this._currentPlayerIndex = startPlayerIndex;
            this._playerPositions = CreatePlayerPositions(gamePlayers);
            this._dodgeBallPositions = CreateDodgeballs(gamePlayers.Count);
            this._playerDodgeBalls = new Dictionary<IGamePlayer, DodgeBall>();
        }

        private Dictionary<DodgeBall, Vector2> CreateDodgeballs(int count)
        {
            Dictionary<DodgeBall, Vector2> positions = new();
            for (int i = 0; i < count; i++)
            {
                Vector2 position = new Vector2(i * 20, AreaSize / 2f);

                positions[new DodgeBall(new Vector2())] = position;
            }

            return positions;
        }

        private List<(IGamePlayer, Vector2)> CreatePlayerPositions(List<IGamePlayer> players)
        {
            List<(IGamePlayer, Vector2)> positions = new();
            for (int i = 0; i < players.Count; i++)
            {
                Vector2 playerPosition = new Vector2(AreaSize * (i % 2), AreaSize * (i + 1 % 2));
                positions.Add((players[i], playerPosition));
            }

            return positions;
        }

        public void Initialize()
        {
            this._currentGameState = this.InternalCreateGameState();
        }

        public IGamePlayer? GetWinner()
        {
            if (this._playerPositions.Count == 1)
            {
                return this._playerPositions[0].Player;
            }

            return null;
        }

        public IGameState GetGameState()
        {
            if (this._currentGameState is null)
            {
                throw new InvalidOperationException($"Game has not been initialized yet!");
            }

            return this._currentGameState;
        }

        public void PlaceMove(IGamePlayer gamePlayer, Move move)
        {
            throw new NotImplementedException();
        }

        public IGamePlayer NextPlayer()
        {
            return this._playerPositions[this._currentPlayerIndex].Player;
        }

        public string Description => $"Dodgeball game between: {string.Join(',', this.Players.Select(p => p.Player.Name))}! With {this._dodgeBallPositions.Count} balls";

        public bool IsDone => this._moveCounter >= MaxMoves || this._playerPositions.Count == 1;

        public List<IGamePlayer> Players => this._players;

        private DodgeBallGameState InternalCreateGameState()
        {
            List<(int Player, Vector2 Position)> positions = new();
            for (int i = 0; i < this._playerPositions.Count; i++)
            {
                positions.Add((i, this._playerPositions[i].Position));
            }

            int currentPlayer = this._currentPlayerIndex;
            bool hasBall = this._playerDodgeBalls.ContainsKey(this._playerPositions[this._currentPlayerIndex].Player);

            return new DodgeBallGameState(positions, currentPlayer, hasBall, this._dodgeBallPositions);
        }

        internal class DodgeBallGameState : IDodgeBallGameState, IGameState
        {
            private List<(int Player, Vector2 Position)> _playerPositions;
            private Vector2 _currentPlayerPosition;
            private bool _currentPlayerHasBall;
            private Dictionary<DodgeBall, Vector2> _dodgeBallPositions;
            internal DodgeBallGameState(List<(int Player, Vector2 Position)> playerPositions, int currentPlayerIndex, bool currentPlayerHasABall, Dictionary<DodgeBall, Vector2> dodgeBallPositions)
            {
                this._playerPositions = playerPositions;
                this._currentPlayerPosition = this._playerPositions[currentPlayerIndex].Position;
                this._currentPlayerHasBall = currentPlayerHasABall;
                this._dodgeBallPositions = dodgeBallPositions;
            }

            public GameType GameType => GameType.DodgeBall;

            public object InternalBoardState => this;

            public bool CanPickUpBall(DodgeBall ball)
            {
                if (this.CurrentPlayerHasABall())
                {
                    return false;
                }

                if (!this._dodgeBallPositions.TryGetValue(ball, out Vector2 position))
                {
                    return false;
                }

                if (Math.Abs((this._currentPlayerPosition - position).Length()) > 2f)
                {
                    return false;
                }
               
                return true;
            }

            public bool CurrentPlayerHasABall() => this._currentPlayerHasBall;

            public Vector2 CurrentPosition() => new Vector2(this._currentPlayerPosition.X, this._currentPlayerPosition.Y);

            public List<(DodgeBall Ball, Vector2 Position)> GetDodgeballsAndPositions() => this._dodgeBallPositions.ToList().Select(p => (p.Key, p.Value)).ToList();

            public List<(int Player, Vector2 Position)> GetPlayerPositions() => this._playerPositions.ToList();

            public bool IsMoveLegal(DodgeBallPlayerMove move)
                =>  move.PlayerAction switch
                {
                    DodgeBallPlayerMove.Action.Move when this.CurrentPlayerHasABall() => false,
                    DodgeBallPlayerMove.Action.Move => true,
                    DodgeBallPlayerMove.Action.PickUp when this.CurrentPlayerHasABall() => false,
                    DodgeBallPlayerMove.Action.PickUp when !this.CanPickUpBall(move.Ball!) => false,
                    DodgeBallPlayerMove.Action.PickUp => true,
                    DodgeBallPlayerMove.Action.Throw when !this.CurrentPlayerHasABall() => false,
                    DodgeBallPlayerMove.Action.Throw => true,
                    DodgeBallPlayerMove.Action.Wait => true,
                    _ => false
                };
        }
    }
}
