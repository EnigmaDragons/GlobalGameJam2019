using System.Collections.Generic;

namespace MonoDragons.GGJ.Gameplay
{
    public class HandDrawn
    {
        public int TurnNumber { get; set; }
        public Player Player { get; set; }
        public List<int> Cards { get; set; }
        public List<int> PlayableCards { get; set; }

        public HandDrawn(int turnNumber, Player player, List<int> cards, List<int> playableCards)
        {
            TurnNumber = turnNumber;
            Player = player;
            Cards = cards;
            PlayableCards = playableCards;
        }
    }
}
