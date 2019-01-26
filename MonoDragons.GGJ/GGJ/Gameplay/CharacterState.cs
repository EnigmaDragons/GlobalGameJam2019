using System.Collections.Generic;
using MonoDragons.Core.EventSystem;

namespace MonoDragons.GGJ.Gameplay
{
    public class CharacterState
    {
        public Player Controller { get; }
        public int HP { get; set; } = 50;
        public Deck Deck { get; }
        public Hand Hand { get; }

        public CharacterState(Player controller, int hp, Deck deck)
        {
            Controller = controller;
            HP = hp;
            Deck = deck;
            Hand = new Hand(controller, deck.DrawCards(3));
        }
    }
}
