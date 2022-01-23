using System.Collections.Generic;

namespace GameMaster.Entities
{
    public class Match
    {
        public Match(List<IGame> games)
        {
            this.Games = games;
        }

        public List<IGame> Games { get; }
    }
}
