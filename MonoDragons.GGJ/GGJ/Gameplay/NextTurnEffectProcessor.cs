using System.Collections.Generic;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Gameplay.Events;

namespace MonoDragons.GGJ.Gameplay
{
    public class NextTurnEffectProcessor
    {
        private List<object> _effects;

        public NextTurnEffectProcessor()
        {
            _effects = new List<object>();
            Event.Subscribe<NextTurnEffectQueued>(e => _effects.Add(e.Event), this);
            Event.Subscribe<TurnStarted>(OnStartOfTurn, this);
        }

        private void OnStartOfTurn(TurnStarted e)
        {
            _effects.ForEach(Event.Publish);
            _effects = new List<object>();
        }
    }
}
