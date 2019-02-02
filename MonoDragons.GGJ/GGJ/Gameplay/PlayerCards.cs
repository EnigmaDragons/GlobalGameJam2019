using System;
using MonoDragons.Core.EventSystem;
using System.Collections.Generic;
using System.Linq;
using MonoDragons.Core;
using MonoDragons.GGJ.Gameplay.Events;

namespace MonoDragons.GGJ.Gameplay
{
    public class PlayerCards
    {
        private readonly Player _player;
        private readonly GameData _data;
        private readonly GameRng _rng;
        private PlayerCardsState _state => _data[_player].Cards;
        private int _currentTurn = -1;

        public PlayerCards(Player player, GameData data, GameRng rng)
        {
            _player = player;
            _data = data;
            _rng = rng;
            _currentTurn = _data.CurrentTurn;
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
            Event.Subscribe<CardSelected>(OnCardSelected, this);
            Event.Subscribe<TurnStarted>(OnTurnStarted, this);
            Event.Subscribe<TurnFinished>(OnTurnFinished, this);
            Event.Subscribe<LevelSetup>(OnLevelSetup, this);
            Event.Subscribe<CardTypeLocked>(OnCardTypeLocked, this);
        }

        private void OnLevelSetup(LevelSetup e)
        {
            _currentTurn = -1;
            DiscardInPlay();
            Reshuffle();
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            _currentTurn = int.MaxValue;
        }

        private void OnCardTypeLocked(CardTypeLocked e)
        {
            if (e.Target == _player && !_state.NextTurnUnplayableTypes.Contains(e.Type))
                _state.NextTurnUnplayableTypes.Add(e.Type);
        }

        private void OnTurnFinished(TurnFinished e)
        {
            DiscardInPlay();
            _state.UnplayableTypes = _state.NextTurnUnplayableTypes;
            _state.NextTurnUnplayableTypes = new List<CardType>();
        }

        private void OnTurnStarted(TurnStarted e)
        {
            if (_currentTurn >= e.TurnNumber)
                return;

            _currentTurn = e.TurnNumber;
            DrawCard(_state.AttackDrawZone, _state.AttackDiscardZone);
            DrawCard(_state.DefendDrawZone, _state.DefendDiscardZone);
            DrawCard(_state.ChargeDrawZone, _state.ChargeDiscardZone);
            DrawCard(_state.CounterDrawZone, _state.CounterDiscardZone);
            if (_state.HandZone.Select(x => _data.AllCards[x]).All(x => _state.UnplayableTypes.Any(y => y == x.Type)))
                DrawPass();
            Event.Publish(new HandDrawn
            (
                _currentTurn,
                _player,
                _state.HandZone,
                _state.HandZone.Where(x => !_state.UnplayableTypes.Contains(_data.Card(x).State.Type)).ToList()
            ));
        }

        private void OnCardSelected(CardSelected e)
        {
            if (e.Player != _player)
                return;

            Play(e.CardId);
            DiscardHand();
        }

        private void DiscardInPlay()
        {
            _state.InPlayZone.ForEach(x =>
            {
                if (_data.AllCards[x].Type == CardType.Attack)
                    _state.AttackDiscardZone.Add(x);
                if (_data.AllCards[x].Type == CardType.Defend)
                    _state.DefendDiscardZone.Add(x);
                if (_data.AllCards[x].Type == CardType.Charge)
                    _state.ChargeDiscardZone.Add(x);
                if (_data.AllCards[x].Type == CardType.Counter)
                    _state.CounterDiscardZone.Add(x);
            });
            _state.InPlayZone.Clear();
        }

        private void DiscardHand()
        {
            _state.HandZone.ForEach(x =>
            {
                if (_data.AllCards[x].Type == CardType.Attack)
                    _state.AttackDiscardZone.Add(x);
                if (_data.AllCards[x].Type == CardType.Defend)
                    _state.DefendDiscardZone.Add(x);
                if (_data.AllCards[x].Type == CardType.Charge)
                    _state.ChargeDiscardZone.Add(x);
                if (_data.AllCards[x].Type == CardType.Counter)
                    _state.CounterDiscardZone.Add(x);
            });
            _state.HandZone.Clear();
        }

        private void DrawCard(List<int> drawPile, List<int> discardPile)
        {
            if (drawPile.Count == 0)
                Reshuffle(drawPile, discardPile);
            var card = _rng.Random(drawPile);
            drawPile.Remove(card);
            _state.HandZone.Add(card);
        }

        private void Reshuffle(List<int> drawPile, List<int> discardPile)
        {
            drawPile.AddRange(discardPile);
            discardPile.Clear();
        }

        private void DrawPass()
        {
            _state.HandZone.Add(_state.PassId);
        }

        private void Play(int cardId)
        {
            if (_state.UnplayableTypes.Contains(State<GameData>.Current.AllCards[cardId].Type))
                throw new Exception("You can't play this");
            var card = _state.HandZone.Single(x => x == cardId);
            _state.InPlayZone.Add(card);
            _state.HandZone.Remove(card);
        }

        private void Reshuffle()
        {
            Reshuffle(_state.AttackDrawZone, _state.AttackDiscardZone);
            Reshuffle(_state.DefendDrawZone, _state.DefendDiscardZone);
            Reshuffle(_state.ChargeDrawZone, _state.ChargeDiscardZone);
            Reshuffle(_state.CounterDrawZone, _state.CounterDiscardZone);
        }
    }
}
