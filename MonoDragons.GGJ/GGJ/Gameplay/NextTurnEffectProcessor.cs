using System.Collections.Generic;
using System.Linq;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Gameplay.Events;

namespace MonoDragons.GGJ.Gameplay
{
    public class NextTurnEffectProcessor
    {
        private readonly GameData _data;

        public NextTurnEffectProcessor(GameData data)
        {
            _data = data;
            Event.Subscribe<NextTurnEffectQueued>(e => _data.NextTurnEffects.Add(e.Event), this);
            Event.Subscribe<TurnStarted>(OnStartOfTurn, this);
        }

        private void OnStartOfTurn(TurnStarted e)
        {
            var effects = _data.NextTurnEffects.ToList();
            _data.NextTurnEffects = new List<object>();
            effects.ForEach(Event.Publish);
        }
    }
}
