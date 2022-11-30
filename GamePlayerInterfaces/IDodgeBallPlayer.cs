using GamePlayerInterfaces.DodgeBall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GamePlayerInterfaces
{
    public interface IDodgeBallPlayer
    {
        public void Initialize(bool isStarting);

        public DodgeBallPlayerMove NextMove(IDodgeBallGameState state);
    }
}
