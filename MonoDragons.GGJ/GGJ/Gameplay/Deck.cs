using System.Collections.Generic;
using System.Linq;

namespace MonoDragons.GGJ.Gameplay
{
    public class Deck
    {
        private readonly List<Card> _deck;
        private readonly List<Card> _discard = new List<Card>();

        public Deck(params Card[] cards)
        {
            _deck = cards.ToList();
        }

        public List<Card> DrawCards(int num)
        {
            var cards = new List<Card>();
            for (var i = 0; i < num; i++)
            {
                if (_deck.Count == 0)
                    Reshuffle();
                var card = _deck.Random();
                _deck.Remove(card);
                cards.Add(card);
            }
            _discard.AddRange(cards);
            return cards;
        } 

        private void Reshuffle()
        {
            _deck.AddRange(_discard);
            _discard.Clear();
        }
    }
}
