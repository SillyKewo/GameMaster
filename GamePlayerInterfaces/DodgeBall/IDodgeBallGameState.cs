using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GamePlayerInterfaces.DodgeBall
{
    public interface IDodgeBallGameState
    {
        public Vector2 CurrentPosition();

        public List<(int Player, Vector2 Position)> GetPlayerPositions();

        public bool CurrentPlayerHasABall();

        public List<(DodgeBall Ball, Vector2 Position)> GetDodgeballsAndPositions();

        public bool IsMoveLegal(DodgeBallPlayerMove move);

        public bool CanPickUpBall(DodgeBall ball);
    }
}
