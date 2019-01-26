using System.Collections.Generic;
using System.Linq;

namespace MonoDragons.GGJ.Gameplay
{
    public class PlayerCardsState
    {
        public List<int> MasterList { get; set; }
        public List<int> DrawZone { get; set; }
        public List<int> HandZone { get; set; }
        public List<int> InPlayZone { get; set; }
        public List<int> DiscardZone { get; set; }

        public PlayerCardsState() : this(new List<int>()) { }
        public PlayerCardsState(params CardState[] s) : this(s.Select(x => x.Id)) { }
        public PlayerCardsState(IEnumerable<int> ids)
        {
            MasterList = ids.ToList();
            DrawZone = ids.ToList();
            HandZone = new List<int>();
            InPlayZone = new List<int>();
            DiscardZone = new List<int>();
        }
    }
}
