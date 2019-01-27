using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Data;
using MonoDragons.GGJ.UiElements.Events;
using System;

namespace MonoDragons.GGJ.Gameplay
{
    public class CardRevealer : IVisualAutomaton
    {
        // TODO: Encapsulate this
        public Optional<CardView> Card { get; set; }
        public bool IsRevealed { get; set; }
        private Transform2 _location;
        private TimerTask _displayTimer;
        private readonly Player _local;
        private readonly Player _player;
        private bool _levelIsFinished;

        public CardRevealer(Player local, Player player, Vector2 location) : this(local, player, location, new Optional<CardView>()) { }
        public CardRevealer(Player local, Player player, Vector2 location, CardView card) : this(local, player, location, new Optional<CardView>(card)) { }
        public CardRevealer(Player local, Player player, Vector2 location, Optional<CardView> card)
        {
            _location = new Transform2(location);
            _local = local;
            _player = player;
            Card = card;
            IsRevealed = local == player;
            Event.Subscribe<CardSelected>(OnCardSelect, this);
            Event.Subscribe<AllCardsSelected>(OnCardsSelected, this);
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
            Event.Subscribe<LevelSetup>(x => _levelIsFinished = false, this);
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            _levelIsFinished = true;
            CleanupRevelations();
        }

        private void OnCardSelect(CardSelected e)
        {
            if (e.Player != _player)
                return;

            ShowCard(e.CardId);
        }

        private void ShowCard(int cardId)
        {
            Card = new Optional<CardView>(Cards.Create(State<GameData>.Current.AllCards[cardId]));
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
            ShowCard(_player == Player.Cowboy ? e.CowboyCard : e.HouseCard);
            _displayTimer = new TimerTask(() =>
            {
                CleanupRevelations();
                if (!_levelIsFinished)
                    Event.Publish(new AnimationEnded());
            }, 2000, false);
            Event.Publish(new AnimationStarted("Show Played Card"));
            Event.Publish(new AllCardsRevealed { TurnNumber = e.TurnNumber, CowboyCard = e.CowboyCard, HouseCard = e.HouseCard });
        }

        private void CleanupRevelations()
        {
            Card = new Optional<CardView>();
            if (_local != _player)
                IsRevealed = false;
        }
    }
}
