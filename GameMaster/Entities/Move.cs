using GameMaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    public class Move
    {
        public Move(List<float> commands, Player player )
        {
            this.Commands = commands;
            this.Player = player;
        }

        public List<float> Commands { get; set; }

        public Player Player { get; set; }
    }
}
