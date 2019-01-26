using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using System;

namespace MonoDragons.GGJ.Gameplay
{
    public class CardRevealer : IVisualAutomaton
    {
        public Optional<Card> Card { get; set; }
        public bool IsRevealed { get; set; }
        private Transform2 _location;
        private TimerTask _displayTimer;

        public CardRevealer(Vector2 location, bool isRevealed = false) : this(location, new Optional<Card>(), isRevealed) { }
        public CardRevealer(Vector2 location, Card card, bool isRevealed = false) : this(location, new Optional<Card>(card), isRevealed) { }
        public CardRevealer(Vector2 location, Optional<Card> card, bool isRevealed)
        {
            _location = new Transform2(location);
            Card = card;
            IsRevealed = isRevealed;
            Event.Subscribe<AllCardsSelected>(OnCardsSelected, this);
        }

        public void Draw(Transform2 parentTransform)
        {
            if (IsRevealed && Card.HasValue)
                Card.Value.Draw(_location);
        }

        public void Update(TimeSpan delta)
        {
            _displayTimer?.Update(delta);   
        }

        private void OnCardsSelected(AllCardsSelected e)
        {
            _displayTimer = new TimerTask(() => Event.Publish(new TurnFinished()), 6000, false);
        }
    }
}
