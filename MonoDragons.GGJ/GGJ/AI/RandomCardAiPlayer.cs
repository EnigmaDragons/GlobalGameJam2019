using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Gameplay;

namespace MonoDragons.GGJ.AI
{
    public class RandomCardAiPlayer
    {
        private readonly Player _player;
        private readonly GameData _data;
        private readonly PlayerCards _cards;

        public RandomCardAiPlayer(Player player, GameData data, PlayerCards cards)
        {
            _player = player;
            _data = data;
            _cards = cards;
            Event.Subscribe<HandDrawn>(OnHandDrawn, this);
        }

        private void OnHandDrawn(HandDrawn e)
        {
            if (e.Player == _player)
                Event.Publish(new CardSelected(e.PlayableCards.Random(), _player));
        }
    }
}