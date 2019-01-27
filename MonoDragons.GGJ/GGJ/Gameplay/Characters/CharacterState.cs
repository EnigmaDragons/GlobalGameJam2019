
namespace MonoDragons.GGJ.Gameplay
{
    public class CharacterState
    {
        public Player Player { get; }
        public int HP { get; set; }
        public int NextAttackBonus { get; set; }
        public PlayerCardsState Cards { get; set; }

        public CharacterState(Player player, int hp, PlayerCardsState cards)
        {
            Player = player;
            HP = hp;
            Cards = cards;
        }
    }
}
