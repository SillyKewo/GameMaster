using Entities;
using GamePlayerInterfaces.DodgeBall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster.Dodgeball
{
    public static class MoveHelper
    {
        public static Move FromDodgeBallMove(DodgeBallPlayerMove move, Player player)
        {
            List<float> commands = new List<float>();

            commands.Add((int)move.PlayerAction);
            commands.Add(move.Direction.X);
            commands.Add(move.Direction.Y);

            return new Move(commands, player);
        }

        public static DodgeBallPlayerMove ToDodgeBallPlayerMove(Move move)
        {
            return (DodgeBallPlayerMove.Action)move.Commands[0] switch
            {
                DodgeBallPlayerMove.Action.Move => DodgeBallPlayerMove.Move(new System.Numerics.Vector2(move.Commands[1], move.Commands[2])),
                DodgeBallPlayerMove.Action.PickUp => DodgeBallPlayerMove.PickUpBall(null),
                DodgeBallPlayerMove.Action.Throw => DodgeBallPlayerMove.ThrowBall(null, new System.Numerics.Vector2(move.Commands[1], move.Commands[2])),
                DodgeBallPlayerMove.Action.Wait => DodgeBallPlayerMove.Wait(),
            };
        }
    }
}
