using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoDragons.GGJ.Gameplay
{
    public class PhaseTransitions : IAutomaton
    {
        private readonly GameData _gameData;
        private Phase _current = Phase.SelectingCards;
        private List<CardSelected> _selections = new List<CardSelected>();

        public PhaseTransitions(GameData gameData)
        {
            _gameData = gameData;
            Event.Subscribe<CardSelected>(CardSelected, this);
        }

        public void Update(TimeSpan delta)
        {
            if (_gameData.CowboyState.HP <= 0)
                Event.Publish(new PlayerDefeated { Winner = Player.House, IsGameOver = true });
            else if (_gameData.HouseState.HP <= 0)
                Event.Publish(new PlayerDefeated { Winner = Player.Cowboy, IsGameOver = false });
        }

        private void CardSelected(CardSelected selection)
        {
            _selections.Add(selection);

            if (_selections.Count >= 2 && _selections.Any(x => x.Player == Player.Cowboy) && _selections.Any(x => x.Player == Player.House))
            {
                Event.Publish(new AllCardsSelected
                {
                    CowboyCard = _selections.First(x => x.Player == Player.Cowboy).CardId,
                    HouseCard = _selections.First(x => x.Player == Player.House).CardId
                });
                _selections = new List<CardSelected>();
            }
        }
    }
}
