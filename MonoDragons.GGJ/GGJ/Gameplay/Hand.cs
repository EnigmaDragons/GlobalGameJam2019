using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay.Events;
using System.Collections.Generic;
using System.Linq;

namespace MonoDragons.GGJ.Gameplay
{
    public class Hand : IVisualControl
    {
        public ClickUIBranch Branch { get; private set; }
        public List<Card> Cards { get; private set; }
        private Player _player;
        
        public Hand(Player player, List<Card> cards)
        {
            _player = player;
            Cards = cards;
            Branch = new ClickUIBranch("Hand", 1);
            for (var i = 0; i < cards.Count; i++)
            {
                var ii = i;
                Branch.Add(new SimpleClickable(new Rectangle(100 + i * (Card.WIDTH + 50), 850 - Card.HEIGHT, Card.WIDTH, Card.HEIGHT), () => CardSelected(ii)));
            }
            Event.Subscribe<CardSelected>(OnCardSelected, this);
        }

        private void OnCardSelected(CardSelected e)
        {
            if (e.Player == _player)
                DiscardAll();
        }

        public void AddCards(List<Card> cards)
        {
            foreach (var card in cards)
                AddCard(card);
        }

        public void AddCard(Card card)
        {
            var index = Cards.Count();
            Branch.Add(new SimpleClickable(new Rectangle(100 + index * (Card.WIDTH + 50), 850 - Card.HEIGHT, Card.WIDTH, Card.HEIGHT), () => CardSelected(index)));
            Cards.Add(card);
        }

        private void CardSelected(int cardIndex)
        {
            Event.Publish(new CardSelected(Cards[cardIndex].Id, _player));
        }

        public void Draw(Transform2 parentTransform)
        {
            for (var i = 0; i < Cards.Count; i++)
                Cards[i].Draw(new Transform2(new Vector2(100 + i * (Card.WIDTH + 50), 850 - Card.HEIGHT)));
        }

        public void DiscardAll()
        {
            Cards.Clear();
            Branch.ClearElements();
        }
    }
}
