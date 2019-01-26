using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Gameplay.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoDragons.GGJ.Gameplay
{
    public class PhaseTransitions : IAutomaton
    {
        private readonly GameData _gameData;
        private readonly Player _local;
        private List<CardSelected> _selections = new List<CardSelected>();

        public PhaseTransitions(GameData gameData, Player local)
        {
            _local = local;
            _gameData = gameData;
            Event.Subscribe<CardSelected>(CardSelected, this);
            Event.Subscribe<TurnFinished>(_ => _gameData.CurrentPhase = Phase.StartingTurn, this);
            Event.Subscribe<TurnStarted>(TurnStarted, this);
        }

        private void TurnStarted(TurnStarted turnStarted)
        {
            _gameData.CurrentPhase = Phase.SelectingCards;
            Event.Publish(new GameDataCanBeSynced(_gameData, _local));
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
                _gameData.CurrentPhase = Phase.ResolvingCards;
                _selections = new List<CardSelected>();
            }
        }
    }
}
