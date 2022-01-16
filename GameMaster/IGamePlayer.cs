using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    public interface IGamePlayer
    {
        public string User { get; }

        public Move NextMove(IGameState gameState);

        public void Initialize(bool isStarting);
    }
}
