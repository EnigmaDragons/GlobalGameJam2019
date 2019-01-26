using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoDragons.GGJ.Gameplay
{
    public class HandView : IVisualControl
    {
        public ClickUIBranch Branch { get; private set; }

        private readonly GameData _data;
        private CharacterState State => _data[_player];
        private readonly List<Card> _cards = new List<Card>();
        private readonly Player _player;
        
        public HandView(Player player, GameData data)
        {
            _player = player;
            _data = data;
            Branch = new ClickUIBranch("Hand", 1);
            Event.Subscribe<CardSelected>(OnCardSelected, this);
            Event.Subscribe<HandDrawn>(OnHandDrawn, this);
        }

        private void OnHandDrawn(HandDrawn e)
        {
            if (e.Player == State.Player)
                AddCards(e.Cards);
        }

        private void OnCardSelected(CardSelected e)
        {
            if (e.Player == State.Player)
                DiscardAll();
        }

        private void AddCards(List<int> cards)
        {
            foreach (var id in cards)
                AddCard(Cards.Create(_data.AllCards[id]));
        }

        private void AddCard(Card card)
        {
            var index = _cards.Count();
            if (!State.Cards.UnplayableTypes.Contains(card.State.Type))
                Branch.Add(new SimpleClickable(new Rectangle(100 + index * (Card.WIDTH + 50), 850 - Card.HEIGHT, Card.WIDTH, Card.HEIGHT), () => CardSelected(index)));
            _cards.Add(card);
        }

        private void CardSelected(int cardIndex)
        {
            Event.Publish(new CardSelected(_cards[cardIndex].Id, _player));
        }

        public void Draw(Transform2 parentTransform)
        {
            for (var i = 0; i < _cards.Count; i++)
                _cards[i].Draw(new Transform2(new Vector2(100 + i * (Card.WIDTH + 50), 850 - Card.HEIGHT)));
        }

        public void DiscardAll()
        {
            _cards.ForEach(x => Event.Unsubscribe(x));
            _cards.Clear();
            Branch.ClearElements();
        }
    }
}
