using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GamePlayerInterfaces.DodgeBall
{
    public class DodgeBallPlayerMove
    {
        public enum Action
        {
            Move,
            PickUp,
            Throw,
            Wait
        }

        private DodgeBallPlayerMove(Action playerAction, DodgeBall? ball, Vector2 direction)
        {
            this.PlayerAction = playerAction;
            this.Direction = direction;
            this.Ball = ball;
        }

        public DodgeBall? Ball { get; }
        public Action PlayerAction { get; }
        public Vector2 Direction { get; }


        public static DodgeBallPlayerMove PickUpBall(DodgeBall dodgeBall)
        {
            return new DodgeBallPlayerMove(Action.PickUp, dodgeBall, new Vector2());
        }

        public static DodgeBallPlayerMove ThrowBall(DodgeBall dodgeBall, Vector2 direction)
        {
            return new DodgeBallPlayerMove(Action.Throw, dodgeBall, direction);
        }

        public static DodgeBallPlayerMove Move(Vector2 direction)
        {
            return new DodgeBallPlayerMove(Action.Move, null, direction);
        }

        public static DodgeBallPlayerMove Wait()
        {
            return new DodgeBallPlayerMove(Action.Wait, null, new Vector2());
        }
    }
}
