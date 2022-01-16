using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    public class Move
    {
        public Move(List<int> commands)
        {
            this.Commands = commands;
        }

        public List<int> Commands { get; set; }
    }
}
