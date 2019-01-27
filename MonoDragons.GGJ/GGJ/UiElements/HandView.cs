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
        private readonly List<CardView> _cards = new List<CardView>();
        private readonly List<bool> _cardIsPlayable = new List<bool>();
        private readonly Player _player;
        private readonly UiImage _chains = new UiImage { Image = "UI/card-chains", Transform = new Transform2(new Vector2(0, 0), new Size2(CardView.WIDTH, CardView.HEIGHT))};
        
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

        private void AddCard(CardView cardView)
        {
            var index = _cards.Count();

            var isPlayable = !State.Cards.UnplayableTypes.Contains(cardView.State.Type);

            if (isPlayable)
                Branch.Add(new SimpleClickable(new Rectangle(100 + index * (CardView.WIDTH + 50), 850 - CardView.HEIGHT, CardView.WIDTH, CardView.HEIGHT), () => CardSelected(index)));
            _cards.Add(cardView);
            _cardIsPlayable.Add(isPlayable);
        }

        private void CardSelected(int cardIndex)
        {
            Event.Publish(new CardSelected(_cards[cardIndex].Id, _player));
        }

        public void Draw(Transform2 parentTransform)
        {
            const int xMargin = 50;
            const int width = CardView.WIDTH + xMargin;
            const int height = CardView.HEIGHT;
            const int xOff = 100;
            const int yOff = 900;
            for (var i = 0; i < _cards.Count; i++)
            {
                var pos = new Transform2(new Vector2(xOff + i * width, yOff - height));
                _cards[i].Draw(pos);
                if (!_cardIsPlayable[i])
                    _chains.Draw(pos);
            }
        }

        public void DiscardAll()
        {
            _cards.ForEach(x => Event.Unsubscribe(x));
            _cards.Clear();
            _cardIsPlayable.Clear();
            Branch.ClearElements();
        }
    }
}
