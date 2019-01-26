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
        private int _currentTurn = 0;
        private int _currentLevel = 0;

        private List<CardSelected> _selections = new List<CardSelected>();

        public PhaseTransitions(GameData gameData)
        {
            _currentLevel = gameData.CurrentLevel;
            _gameData = gameData;
            Event.Subscribe<TurnStarted>(OnTurnStarted, this);
            Event.Subscribe<CardSelected>(CardSelected, this);
            Event.Subscribe<TurnFinished>(OnTurnFinished, this);
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
            Event.Subscribe<LevelSetup>(OnLevelSetup, this);
        }

        private void OnLevelSetup(LevelSetup e)
        {
            _gameData.CurrentLevel = e.CurrentLevel;
            _gameData.CurrentTurn = 0;
            _currentLevel = e.CurrentLevel;
            Event.Publish(new TurnStarted { TurnNumber = _gameData.CurrentTurn });
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            if (!e.IsGameOver)
                _gameData.CurrentPhase = Phase.LeavingLevel;
        }

        private void OnTurnStarted(TurnStarted e)
        {
            if (e.TurnNumber < _currentTurn)
                return;

            _gameData.CurrentTurn = e.TurnNumber;
            _gameData.CurrentPhase = Phase.StartingTurn;
            _currentTurn = e.TurnNumber;
        }

        private void OnTurnFinished(TurnFinished e)
        {
            if (e.TurnNumber < _currentTurn)
                return; 

            _gameData.CurrentTurn = e.TurnNumber + 1;
            Event.Publish(new TurnStarted { TurnNumber = _gameData.CurrentTurn });
        }

        public void Update(TimeSpan delta)
        {
            if (_currentLevel != _gameData.CurrentLevel) 
                return;
            
            if (_gameData.CowboyState.HP <= 0 || _gameData.HouseState.HP <= 0)
                _currentLevel++;
            
            if (_gameData.CowboyState.HP <= 0)
                Event.Publish(new PlayerDefeated {LevelNumber = _currentLevel, Winner = Player.House, IsGameOver = true});
            else if (_gameData.HouseState.HP <= 0)
                Event.Publish(new PlayerDefeated {LevelNumber = _currentLevel, Winner = Player.Cowboy, IsGameOver = false});
        }

        private void CardSelected(CardSelected selection)
        {
            _selections.Add(selection);

            if (_selections.Count >= 2 && _selections.Any(x => x.Player == Player.Cowboy) && _selections.Any(x => x.Player == Player.House))
            {
                Event.Publish(new AllCardsSelected
                {
                    TurnNumber = _gameData.CurrentTurn,
                    CowboyCard = _selections.First(x => x.Player == Player.Cowboy).CardId,
                    HouseCard = _selections.First(x => x.Player == Player.House).CardId
                });
                _selections = new List<CardSelected>();
            }
        }
    }
}
