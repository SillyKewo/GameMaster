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

        static void Main(string[] args)
        {
            AllocConsole();

            // RunDiceWars();

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
