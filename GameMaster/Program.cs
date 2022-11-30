using GameMaster.DataAccessLayer;
using GameMaster.Entities;
using Hexagonal;
using HexagonalTest;
using HexagonalTest.PlayerAPI;
using HexagonalTest.Players;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using static GameMaster.PlayerInitializationHelper;

namespace GameMaster
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        // TODO: Move output folder to configurations.
        public const string OutputFolder = @"C:\Users\theke\Documents\TicTacToeResults\";
            
        static void Main(string[] args)
        {
            AllocConsole();

            // RunDiceWars();

            // Read the current tournament configurations.
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

            // Create a tournament manager for each game type.
            List<TournamentManager> tournamentManagers = new List<TournamentManager>();

            foreach (var config in configurations)
            {
                List<PlayerActivator> playerActivators = InitializePlayers(config.PlayerFolder, config.GameType);
                switch (config.GameType)
                {
                    case GameType.TicTacToe:
                        TournamentManager tournamentManager = new TournamentManager(playerActivators, config, (p, s) => TicTacToeGame.Create(p, s));
                        tournamentManagers.Add(tournamentManager);
                        break;

                    case GameType.DodgeBall:
                        TournamentManager dodgeBallTournament = new TournamentManager(playerActivators, config, (p, s) => new DodgeBallGame(p, s));
                        tournamentManagers.Add(dodgeBallTournament);
                        break;

                    case GameType.RockPaperScissors:
                        break;
                    default:
                        break;
                }
            }

            // Play each tournament.
            List<TournamentResult> results = new List<TournamentResult>();
            foreach (var tournament in tournamentManagers)
            {
                results.Add(tournament.PlayTournament());
            }


            // Save the results.
            TournamentResultDataMapperXml dataMapper = new TournamentResultDataMapperXml(OutputFolder);
            dataMapper.SaveTournamentsForDate(results, DateTime.UtcNow);

        }

        static void RunDiceWars()
        {
            RandomGenerator.getInstance().initialize();
            
            bool interactiveGuiMode = false;
            if (interactiveGuiMode)
            {
                Console.WriteLine("Starting Form...");

                System.Windows.Forms.Application.Run(new HexagonalTest.MainWIndow());
            }
            else
            {
                int sizeOfBoard = 8;
                var board = new Builder.BoardBuilder()
                    .witHeight(sizeOfBoard)
                    .withWidht(sizeOfBoard)
                    .withSide(25)
                    .withPlayerLogics(new List<IDiceWarsPlayerLogic>
                    {
                        new BlockchainPrepper(),
                        new AlphaRandom(),
                        new DeepRandom(),
                        new QuantumRevenge()
                    })
                    .build();

                board.nextPlayer();

                Console.WriteLine(board.getCurrentPlayerColor() + " has won!");

                System.Windows.Forms.Application.Run(new Fight(board, sizeOfBoard));
            }
        }
    }
}
