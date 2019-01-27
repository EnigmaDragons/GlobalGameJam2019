
using System.Collections.Generic;

namespace MonoDragons.GGJ.Gameplay
{
    public class CharacterState
    {
        public Player Player { get; }
        public int HP { get; set; }
        public PlayerCardsState Cards { get; set; }

        public int NextAttackBonus { get; set; }
        public int IncomingDamage { get; set; }
        public int AvailableBlock { get; set; }
        public List<object> OnDamaged { get; set; }
        public List<object> OnNotDamaged { get; set; }
        public List<object> OnDamageBlocked { get; set; }
        public List<object> OnDamageNotBlocked { get; set; }
        public List<decimal> DamageTakenMultipliers { get; set; }
        public List<decimal> BlockRecievedMultiplier { get; set; }
        public List<Status> Statuses { get; set; }
        public int Energy { get; set; }

        private CharacterState() { }

        public CharacterState(Player player, int hp, PlayerCardsState cards)
        {
            Player = player;
            HP = hp;
            Cards = cards;
            OnDamaged = new List<object>();
            OnNotDamaged = new List<object>();
            OnDamageBlocked = new List<object>();
            OnDamageNotBlocked = new List<object>();
            DamageTakenMultipliers = new List<decimal>();
            BlockRecievedMultiplier = new List<decimal>();
            Statuses = new List<Status>();
        }
    }
}
