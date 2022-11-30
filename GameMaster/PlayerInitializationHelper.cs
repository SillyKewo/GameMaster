using Entities;
using GameMaster.Dodgeball;
using GamePlayerInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    public static class PlayerInitializationHelper
    {
        /// <summary>
        /// Activator class for creating <see cref="IGamePlayer"/> instances
        /// </summary>
        public class PlayerActivator
        {
            private Assembly _assembly;
            private Type _playerInterfaceType;
            private string _user;
            private Func<object, string, IGamePlayer> _wrapperCreator;

            public PlayerActivator(Assembly executionAssembly, Type playerType, string user, Func<object, string, IGamePlayer> wrapperCreator)
            {
                this._assembly = executionAssembly;
                this._playerInterfaceType = playerType;
                this._user = user;
                this._wrapperCreator = wrapperCreator;
            }

            public IGamePlayer CreateNewPlayer()
            {
                Type externalCodeEvent = this._assembly.ExportedTypes
                    .Where(x => x.GetInterfaces().Contains(this._playerInterfaceType)).First();
                object? instance = Activator.CreateInstance(
                        externalCodeEvent
                    );

                if (instance is null)
                {
                    throw new NullReferenceException($"Could not load instance from assembly: {this._assembly.FullName}, type: {this._playerInterfaceType.Name}");
                }

                return this._wrapperCreator(instance, this._user);
            }
        }


        public static List<PlayerActivator> InitializePlayers(string folderPath, GameType gameType)
        {
            string[] directories = Directory.GetDirectories(folderPath);

            List<PlayerActivator> playerActivators = new List<PlayerActivator>();

            foreach(string dir in directories)
            {
                var user = dir.Split('\\').Last();

                var dllFiles = Directory.GetFiles(dir);

                Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllFiles[0]);
                PlayerActivator playerActivator = gameType switch
                {
                    GameType.TicTacToe => new PlayerActivator(assembly, gameType.GetPlayerType(), user, (o, s) => new TicTacToePlayerWrapper(o, new Player(s))),
                    GameType.DodgeBall => new PlayerActivator(assembly, gameType.GetPlayerType(), user, (o, s) => new DodgeBallPlayerWrapper(o, new Player(s))),
                    GameType.RockPaperScissors => throw new NotImplementedException(),
                    _ => throw new NotImplementedException()
                };

                playerActivators.Add(playerActivator);
            }

            return playerActivators;
        }
    }
}
