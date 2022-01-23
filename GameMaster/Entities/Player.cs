using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster.Entities
{
    public class Player
    {
        public Player(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public override bool Equals(object? obj)
        {
            return obj is Player player &&
                   Name == player.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
