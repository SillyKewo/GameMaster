namespace Entities
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
