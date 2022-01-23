using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster.DataAccessLayer
{
    public class MoveDTO
    {
        public string Player { get; set; }
        public List<int> Commands { get; set; }
    }
}
