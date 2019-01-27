using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Gameplay;

namespace MonoDragons.GGJ.AI
{
    public class RandomCardAiPlayer
    {
        private readonly Player _player;
        private readonly GameData _data;
        private readonly PlayerCards _cards;

        public RandomCardAiPlayer(Player player, GameData data)
        {
            _player = player;
            _data = data;
            _cards = new PlayerCards(player, data);
            Event.Subscribe<HandDrawn>(OnHandDrawn, this);
        }

        private void OnHandDrawn(HandDrawn e)
        {
            if (e.Player == _player)
                Event.Publish(new CardSelected(e.PlayableCards.Random(), _player));
        }
    }
}