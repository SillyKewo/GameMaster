using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameMaster.PlayerInitializationHelper;

namespace GameMaster
{
    public class TournamentManager
    {
        private List<PlayerActivator> _playerActivators;
        private VersusMode _mode;
        private Func<IGamePlayer, IGamePlayer, IGame> _gameCreator;

        public TournamentManager(List<PlayerActivator> playerActivators, VersusMode versusMode, Func<IGamePlayer, IGamePlayer, IGame> gameCreator)
        {
            this._playerActivators = playerActivators;
            this._mode = versusMode;
            this._gameCreator = gameCreator;
        }


        public List<string> StartTournament()
        {
            var games = CreateGames();
            List<string> results = new List<string>();
            foreach (IGame game in games)
            {
                GameManager manager = new GameManager(game);

                manager.Play();

                results.Add(manager.GetResult());
            }

            return results;
        }


        private List<IGame> CreateGames()
        {
            List<IGame> games = new List<IGame>();
            switch (this._mode)
            {
                case VersusMode.RoundRobin:
                    for (int i = 0; i < this._playerActivators.Count - 1; i++)
                    {
                        for (int j = i+1; j < this._playerActivators.Count; j++)
                        {
                            games.Add(this._gameCreator(this._playerActivators[i].CreateNewPlayer(), this._playerActivators[j].CreateNewPlayer()));
                        }
                    }
                    break;
                default:
                    break;
            }

            return games;
        }

    }
}
