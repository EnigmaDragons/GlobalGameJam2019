using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Data;
using MonoDragons.GGJ.Gameplay.Events;
using MonoDragons.GGJ.Gameplay;

namespace MonoDragons.GGJ.UiElements
{
    public class CardRevealer : IVisualAutomaton
    {
        // TODO: Encapsulate this
        public Optional<CardView> Card { get; set; }
        private readonly CardView _thinking = new CardView(new CardState { Id = -1, CardName = CardName.None, Type = CardType.Pass }, "Thinking");
        public bool IsRevealed { get; set; }
        private bool _opponentHasChosen;
        private bool _waitingForOpponent;
        private Transform2 _location;
        private TimerTask _displayTimer;
        private CardFightView _cardFightView;
        private readonly Player _local;
        private readonly Player _player;
        private readonly GameData _data;
        private bool _levelIsFinished;

        public CardRevealer(GameData data, Player local, Player player, Vector2 location) : this(data, local, player, location, new Optional<CardView>()) { }
        public CardRevealer(GameData data, Player local, Player player, Vector2 location, CardView card) : this(data, local, player, location, new Optional<CardView>(card)) { }
        public CardRevealer(GameData data, Player local, Player player, Vector2 location, Optional<CardView> card)
        {
            _location = new Transform2(location);
            _local = local;
            _player = player;
            _data = data;
            Card = card;
            IsRevealed = local == player;
            Event.Subscribe<CardSelected>(OnCardSelect, this);
            Event.Subscribe<CardsProcessed>(OnCardsProcessed, this);
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
            Event.Subscribe<TurnStarted>(x => _waitingForOpponent = true, this);
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
            {
                _opponentHasChosen = true;
                _waitingForOpponent = false;
            }
            else
                ShowCard(e.CardId);
        }

        private void ShowCard(int cardId)
        {
            Card = new Optional<CardView>(Cards.Create(State<GameData>.Current.AllCards[cardId]));
        }

        public void Draw(Transform2 parentTransform)
        {
            var t = parentTransform + _location;
            if (IsRevealed && Card.HasValue)
            {
                Card.Value.Draw(t);
                _cardFightView?.Draw(t);
            }
            if (_player != _local && _waitingForOpponent && !_opponentHasChosen)
                _thinking.Draw(t);
        }

        public void Update(TimeSpan delta)
        {
            _cardFightView?.Update(delta);   
        }

        private void OnCardsProcessed(CardsProcessed e)
        {
            IsRevealed = true;
            ShowCard(_player == Player.Cowboy ? e.CowboyCard : e.HouseCard);
            _cardFightView = new CardFightView(Card.Value, _data[_player], _data[_player == Player.Cowboy ? Player.House : Player.Cowboy], _player == Player.House);
            _cardFightView.Start(() =>
            {
                CleanupRevelations();
                //I know this should be on phase trasition but that class is exetremely confusing
                Event.Publish(new CardResolutionBegun { TurnNumber = e.TurnNumber });
            });
            Event.Publish(new AllCardsRevealed { TurnNumber = e.TurnNumber, CowboyCard = e.CowboyCard, HouseCard = e.HouseCard });
        }

        private void CleanupRevelations()
        {
            _opponentHasChosen = false;
            _waitingForOpponent = false;
            Card = new Optional<CardView>();
            if (_local != _player)
                IsRevealed = false;
        }
    }
}
