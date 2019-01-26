using MonoDragons.Core.EventSystem;
using System.Collections.Generic;
using System.Linq;
using MonoDragons.GGJ.Data;

namespace MonoDragons.GGJ.Gameplay
{
    public class PlayerCards
    {
        private readonly Player _player;
        private readonly PlayerCardsState _state;
        private int _currentTurn = -1;

        public PlayerCards(Player player, PlayerCardsState state)
        {
            _player = player;
            _state = state;          
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
            Event.Subscribe<CardSelected>(OnCardSelected, this);
            Event.Subscribe<TurnStarted>(OnTurnStarted, this);
            Event.Subscribe<TurnFinished>(OnTurnFinished, this);
            Event.Subscribe<LevelSetup>(OnLevelSetup, this);
        }

        private void OnLevelSetup(LevelSetup e)
        {
            _currentTurn = -1;
            _state.DiscardZone.AddRange(_state.InPlayZone);
            Reshuffle();
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            _currentTurn = int.MaxValue;
        }

        private void OnTurnFinished(TurnFinished e)
        {
            _state.DiscardZone.AddRange(_state.InPlayZone);
            _state.InPlayZone.Clear();
        }

        private void OnTurnStarted(TurnStarted e)
        {
            if (_currentTurn >= e.TurnNumber)
                return;

            _currentTurn = e.TurnNumber;
            DrawPass();
            DrawCards(3);
        }

        private void OnCardSelected(CardSelected e)
        {
            if (e.Player != _player)
                return;

            Play(e.CardId);
            DiscardAll();
        }

        private void DiscardAll()
        {
            _state.DiscardZone.AddRange(_state.HandZone);
            _state.HandZone.Clear();
        }

        private void DrawPass()
        {
            if (_state.DrawZone.Contains(_state.PassId))
            {
                _state.DrawZone.Remove(_state.PassId);
                _state.HandZone.Add(_state.PassId);
            }
            else if (_state.DiscardZone.Contains(_state.PassId))
            {
                _state.DiscardZone.Remove(_state.PassId);
                _state.HandZone.Add(_state.PassId);
            }
        }

        private void DrawCards(int num)
        {
            var cards = new List<int>();
            for (var i = 0; i < num; i++)
            {
                if (_state.DrawZone.Count == 0)
                    Reshuffle();
                var card = _state.DrawZone.Random();
                _state.DrawZone.Remove(card);
                cards.Add(card);
            }
            _state.HandZone.AddRange(cards);
            Event.Publish(new HandDrawn { TurnNumber = _currentTurn, Player = _player, Cards = _state.HandZone.ToList() });
        } 

        private void Play(int cardId)
        {
            var card = _state.HandZone.Single(x => x == cardId);
            _state.InPlayZone.Add(card);
            _state.HandZone.Remove(card);
        }

        private void Reshuffle()
        {
            _state.DrawZone.AddRange(_state.DiscardZone);
            _state.DiscardZone.Clear();
        }
    }
}
