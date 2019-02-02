using System.Collections.Generic;
using System.Linq;

namespace MonoDragons.GGJ.Gameplay
{
    public class PlayerCardsState
    {
        public List<int> MasterList { get; set; }
        public List<int> AttackDrawZone { get; set; }
        public List<int> DefendDrawZone { get; set; }
        public List<int> ChargeDrawZone { get; set; }
        public List<int> CounterDrawZone { get; set; }
        public List<int> HandZone { get; set; }
        public List<int> InPlayZone { get; set; }
        public List<int> AttackDiscardZone { get; set; }
        public List<int> DefendDiscardZone { get; set; }
        public List<int> ChargeDiscardZone { get; set; }
        public List<int> CounterDiscardZone { get; set; }
        public List<CardType> NextTurnUnplayableTypes { get; set; }
        public List<CardType> UnplayableTypes { get; set; }
        public int HandSizeModifier { get; set; }
        public int PassId { get; }

        private PlayerCardsState() { }
        public PlayerCardsState(params CardState[] s) : this(s.ToList()) { }
        public PlayerCardsState(List<CardState> s) : this(s.First(x => x.Type == CardType.Pass).Id, 
            s.Where(x => x.Type == CardType.Attack).Select(x => x.Id),
            s.Where(x => x.Type == CardType.Defend).Select(x => x.Id),
            s.Where(x => x.Type == CardType.Charge).Select(x => x.Id),
            s.Where(x => x.Type == CardType.Counter).Select(x => x.Id)) { }
        public PlayerCardsState(int passId, IEnumerable<int> attackIds, IEnumerable<int> defendIds, IEnumerable<int> chargeIds, IEnumerable<int> counterIds)
        {
            AttackDrawZone = attackIds.ToList();
            DefendDrawZone = defendIds.ToList();
            ChargeDrawZone = chargeIds.ToList();
            CounterDrawZone = counterIds.ToList();
            MasterList = attackIds.Concat(defendIds).Concat(chargeIds).Concat(counterIds).Concat(new List<int> { passId }).ToList();
            HandZone = new List<int>();
            InPlayZone = new List<int>();
            AttackDiscardZone = new List<int>();
            DefendDiscardZone = new List<int>();
            ChargeDiscardZone = new List<int>();
            CounterDiscardZone = new List<int>();
            NextTurnUnplayableTypes = new List<CardType>();
            UnplayableTypes = new List<CardType>();
            PassId = passId;
        }
    }
}
