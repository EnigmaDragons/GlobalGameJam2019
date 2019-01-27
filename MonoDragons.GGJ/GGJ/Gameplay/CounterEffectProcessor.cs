using System.Collections.Generic;
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

        public CounterEffectProcessor(GameData data)
        {
            _data = data;
            _effects = new List<CounterEffectQueued>();
            Event.Subscribe<CardSelected>(OnCardSelected, this);
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

        private void OnCountersProcessed(CountersProcessed e)
        {
            _effects.ForEach(x =>
            {
                if (x.Type == (x.Caster == Player.Cowboy ? _houseSelectedType : _cowboySelectedType))
                    Event.Publish(x.Event);
            });
            _effects = new List<CounterEffectQueued>();
        }
    }
}
