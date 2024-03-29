﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GameMaster.PlayerInitializationHelper;

namespace GameMaster
{
    /// <summary>
    /// Manager responsible for simulating a tournament, tournaments consist of collection of <see cref="Match"/>es, which each may have one or more <see cref="IGame"/>s.
    /// Each tournament is tied to a specific game type.
    /// </summary>
    public class TournamentManager
    {
        private List<PlayerActivator> _playerActivators;
        private GameSetupConfiguration _config;
        private Func<List<IGamePlayer>, int, IGame> _gameCreator;
        private object _lock = new object();

        public TournamentManager(List<PlayerActivator> playerActivators, GameSetupConfiguration config, Func<List<IGamePlayer>, int, IGame> gameCreator)
        {
            this._playerActivators = playerActivators;
            this._config = config;
            this._gameCreator = gameCreator;
        }

        /// <summary>
        /// Plays out the tournament.
        /// </summary>
        /// <returns>the <see cref="TournamentResult"/>.</returns>
        public TournamentResult PlayTournament()
        {
            List<Match> matches = CreateMatches();
            List<MatchResult> matchResults = new List<MatchResult>();
            Parallel.ForEach(matches, match =>
            {
                List<GameResult> gameResults = new List<GameResult>();
                foreach (IGame game in match.Games)
                {
                    GameManager manager = new GameManager(game, this._config.TimeOutSec);

                    gameResults.Add(manager.Play());
                }

                lock (this._lock)
                {
                    matchResults.Add(new MatchResult(gameResults, this._config.GameBoardConfiguration!));
                }
            });

            return new TournamentResult(matchResults, DateTime.UtcNow, this._config.GameType, this._config.VersusMode);
        }

        private List<Match> CreateMatches()
        {
            List<Match> matches = new List<Match>();
            switch (this._config.VersusMode)
            {
                case VersusMode.RoundRobin:
                    for (int i = 0; i < this._playerActivators.Count - (this._config.PlayersPerGame - 1); i++)
                    {
                        for (int j = i + (this._config.PlayersPerGame - 1); j < this._playerActivators.Count; j++)
                        {

                            List<IGamePlayer> gamePlayers = new List<IGamePlayer>();
                            gamePlayers.Add(this._playerActivators[i].CreateNewPlayer());

                            for (int m = 0; m < this._config.PlayersPerGame - 1; m++)
                            {
                                gamePlayers.Add(this._playerActivators[j+m].CreateNewPlayer());
                            }

                            List<IGame> games = new List<IGame>();

                            for (int k = 0; k < this._config.RoundsPerMatch; k++)
                            {
                                int startPlayer = k % gamePlayers.Count;
                                games.Add(this._gameCreator(gamePlayers, startPlayer));
                            }
                            matches.Add(new Match(games));
                        }
                    }
                    break;
                default:
                    break;
            }

            return matches;
        }

    }
}
