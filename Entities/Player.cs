using System.Diagnostics;

namespace Entities
{
    [DebuggerDisplay("Name = {Name}")]
    public class Player
    {
        public Player(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public override bool Equals(object? obj)
        {
            return (obj is Player player &&
                   Name == player.Name);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public static bool operator ==(Player left, Player right)
        {
            if (left is null || right is null)
            {
                return false;
            }
            
            return left.Equals(right);
        }

        public static bool operator !=(Player left, Player right)
        {
            if (left is null || right is null)
            {
                return false;
            }

            return !(left == right);
        }
    }
}
