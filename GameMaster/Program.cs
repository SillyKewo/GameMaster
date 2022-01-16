using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using static GameMaster.PlayerInitializationHelper;

namespace GameMaster
{
    class Program
    {
        static void Main(string[] args)
        {
            var serializer = new XmlSerializer(typeof(List<GameSetupConfiguration>));
            List<GameSetupConfiguration>? configurations;

            using (var reader = XmlReader.Create(@"./Configurations/Configuration.xml"))
            {
                configurations = (List<GameSetupConfiguration>?)serializer.Deserialize(reader);
            }

            if (configurations is null)
            {
                throw new NullReferenceException($"configurations couldn't be parsed!");
            }

            List<TournamentManager> tournamentManagers = new List<TournamentManager>();

            foreach (var config in configurations)
            {
                switch (config.GameType)
                {
                    case GameType.TicTacToe:
                        List<PlayerActivator> playerActivators = PlayerInitializationHelper.InitializePlayers(config.PlayerFolder, config.GameType);
                        TournamentManager tournamentManager = new TournamentManager(playerActivators, config.VersusMode, (p1, p2) => new TicTacToeGame(p1, p2));
                        tournamentManagers.Add(tournamentManager);

                        break;
                    case GameType.RockPaperScissors:
                        break;
                    default:
                        break;
                }
            }


            foreach (var tournament in tournamentManagers)
            {
                var res = tournament.StartTournament();
            }
            



        }
    }
}
