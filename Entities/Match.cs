using System.Collections.Generic;

namespace Entities
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
