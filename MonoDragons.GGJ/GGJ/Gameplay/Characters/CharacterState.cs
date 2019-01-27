
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
        public List<object> OnNotDamaged { get; set; }
        public List<int> DamageTakenMultipliers { get; set; }

        public CharacterState(Player player, int hp, PlayerCardsState cards)
        {
            Player = player;
            HP = hp;
            Cards = cards;
            OnNotDamaged = new List<object>();
            DamageTakenMultipliers = new List<int>();
        }
    }
}
