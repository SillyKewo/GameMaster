using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GamePlayerInterfaces.DodgeBall
{
    public class DodgeBall
    {
        private Vector2 _velocity;

        public DodgeBall(Vector2 velocity)
        {
            this._velocity = velocity;
        }

        public Vector2 Velocity => this._velocity;

        public bool IsMoving => this._velocity.Length() == 0.0;
    }
}
