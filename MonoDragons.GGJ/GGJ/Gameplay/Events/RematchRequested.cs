using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.GGJ.Gameplay.Events
{
    public class RematchRequested
    {
        public Player Player { get; set; }

        public RematchRequested(Player player)
        {
            Player = player;
        }
    }
}
