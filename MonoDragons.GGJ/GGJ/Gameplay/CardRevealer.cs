using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Data;
using System;

namespace MonoDragons.GGJ.Gameplay
{
    public class CardRevealer : IVisualAutomaton
    {
        // TODO: Encapsulate this
        public Optional<Card> Card { get; set; }
        public bool IsRevealed { get; set; }
        private Transform2 _location;
        private TimerTask _displayTimer;
        private readonly Player _local;
        private readonly Player _player;

        public CardRevealer(Player local, Player player, Vector2 location, bool isRevealed = false) : this(local, player, location, new Optional<Card>(), isRevealed) { }
        public CardRevealer(Player local, Player player, Vector2 location, Card card, bool isRevealed = false) : this(local, player, location, new Optional<Card>(card), isRevealed) { }
        public CardRevealer(Player local, Player player, Vector2 location, Optional<Card> card, bool isRevealed)
        {
            _location = new Transform2(location);
            _local = local;
            _player = player;
            Card = card;
            IsRevealed = isRevealed;
            Event.Subscribe<CardSelected>(OnCardSelect, this);
            Event.Subscribe<TurnFinished>(OnTurnFinished, this);
            Event.Subscribe<AllCardsSelected>(OnCardsSelected, this);
        }

        private void OnTurnFinished(TurnFinished obj)
        {
            Card = new Optional<Card>();
            if (_local != _player)
                IsRevealed = false;            
        }

        private void OnCardSelect(CardSelected e)
        {
            if (e.Player != _player)
                return;

            Card = new Optional<Card>(Cards.GetCardById(e.CardId));
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
            IsRevealed = true;
            _displayTimer = new TimerTask(() => Event.Publish(new TurnFinished { TurnNumber = e.TurnNumber }), 2000, false);
        }
    }
}
