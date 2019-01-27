using System.Linq;
using MonoDragons.Core.EventSystem;

namespace MonoDragons.GGJ.Gameplay
{
    public class StatusProcessor
    {
        private readonly GameData _data;

        public StatusProcessor(GameData data)
        {
            _data = data;
            Event.Subscribe<TurnStarted>(_ => OnTurnStart(), this);
        }

        private void OnTurnStart()
        {
            _data.CowboyState.Statuses.SelectMany(x => x.Events).ForEach(Event.Publish);
            _data.HouseState.Statuses.SelectMany(x => x.Events).ForEach(Event.Publish);
        }
    }
}
