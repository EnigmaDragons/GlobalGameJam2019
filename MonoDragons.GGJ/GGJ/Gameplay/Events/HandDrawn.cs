using System.Collections.Generic;

namespace MonoDragons.GGJ.Gameplay
{
    public class HandDrawn
    {
        public int TurnNumber { get; set; }
        public Player Player { get; set; }
        public List<int> Cards { get; set; }
    }
}
