using System;
using System.Linq;
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
            Event.Subscribe<CardSelected>(OnCardSelected, this);
        }
        private void OnCardSelected(CardSelected e)
        {
            if (e.Player != _player)
            {
                var card = _data[_player].Cards.HandZone.Where(
                    x => !_data[_player].Cards.UnplayableTypes.Contains(_data.Card(x).State.Type)).ToList().Random();
                Event.Publish(new CardSelected(card, _player));
            }
        }
    }
}