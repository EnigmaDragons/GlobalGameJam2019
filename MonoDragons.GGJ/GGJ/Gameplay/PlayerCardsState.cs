using System.Collections.Generic;
using System.Linq;
using MonoDragons.GGJ.Data;

namespace MonoDragons.GGJ.Gameplay
{
    public class PlayerCardsState
    {
        public List<int> MasterList { get; set; }
        public List<int> DrawZone { get; set; }
        public List<int> HandZone { get; set; }
        public List<int> InPlayZone { get; set; }
        public List<int> DiscardZone { get; set; }
        public List<CardType> NextTurnUnplayableTypes { get; set; }
        public List<CardType> UnplayableTypes { get; set; }
        public int HandSizeModifier { get; set; }
        public int PassId { get; }

        private PlayerCardsState() { }
        public PlayerCardsState(List<CardState> s) : this(s.First(x => x.CardName == CardName.CowboyPass || x.CardName == CardName.HousePass).Id, s.Select(x => x.Id)) { }
        public PlayerCardsState(params CardState[] s) : this(s.First(x => x.CardName == CardName.CowboyPass || x.CardName == CardName.HousePass).Id, s.Select(x => x.Id)) { }
        public PlayerCardsState(int passId, IEnumerable<int> ids)
        {
            MasterList = ids.ToList();
            DrawZone = ids.Where(x => x != passId).ToList();
            HandZone = new List<int>();
            InPlayZone = new List<int>();
            DiscardZone = new List<int> { passId };
            NextTurnUnplayableTypes = new List<CardType>();
            UnplayableTypes = new List<CardType>();
            PassId = passId;
        }
    }
}
