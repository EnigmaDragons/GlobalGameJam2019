﻿using System.Collections.Generic;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Gameplay.Events;

namespace MonoDragons.GGJ.Gameplay
{
    public class CounterEffectProcessor
    {
        private readonly GameData _data;
        private List<CounterEffectQueued> _effects;
        private CardType _cowboySelectedType;
        private CardType _houseSelectedType;
        private bool _shouldLockCowboysLastType;
        private bool _shouldLockHousesLastType;

        public CounterEffectProcessor(GameData data)
        {
            _data = data;
            _effects = new List<CounterEffectQueued>();
            Event.Subscribe<CardSelected>(OnCardSelected, this);
            Event.Subscribe<LastPlayedTypeLocked>(OnLastPlayedTypeLocked, this);
            Event.Subscribe<CounterEffectQueued>(e => _effects.Add(e), this);
            Event.Subscribe<CountersProcessed>(OnCountersProcessed, this);
        }

        private void OnCardSelected(CardSelected e)
        {
            if (e.Player == Player.Cowboy)
                _cowboySelectedType = _data.AllCards[e.CardId].Type;
            else
                _houseSelectedType = _data.AllCards[e.CardId].Type;
        }

        private void OnLastPlayedTypeLocked(LastPlayedTypeLocked e)
        {
            if (e.Target == Player.Cowboy)
                _shouldLockCowboysLastType = true;
            if (e.Target == Player.House)
                _shouldLockHousesLastType = true;
        }

        private void OnCountersProcessed(CountersProcessed e)
        {
            if (_shouldLockCowboysLastType && _cowboySelectedType != CardType.Pass)
                Event.Publish(new CardTypeLocked { Target = Player.Cowboy, Type = _cowboySelectedType });
            if (_shouldLockHousesLastType && _houseSelectedType != CardType.Pass)
                Event.Publish(new CardTypeLocked { Target = Player.House, Type = _houseSelectedType });
            _shouldLockCowboysLastType = false;
            _shouldLockHousesLastType = false;

            _effects.ForEach(x =>
            {
                if (x.Type == (x.Caster == Player.Cowboy ? _houseSelectedType : _cowboySelectedType))
                    Event.Publish(x.Event);
            });
            _effects = new List<CounterEffectQueued>();
        }
    }
}
