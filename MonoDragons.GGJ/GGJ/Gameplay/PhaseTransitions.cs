using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Gameplay.Events;
using MonoDragons.GGJ.UiElements.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoDragons.GGJ.Gameplay
{
    public class PhaseTransitions : IAutomaton
    {
        private readonly GameData _gameData;
        private int _currentLevel = 0;
        private int _animationsPending = 0;

        private List<CardSelected> _selections = new List<CardSelected>();

        public PhaseTransitions(GameData gameData)
        {
            _currentLevel = gameData.CurrentLevel;
            _gameData = gameData;
            Event.Subscribe<CardSelected>(CardSelected, this);
            Event.Subscribe<AnimationStarted>(e => _animationsPending++, this);
            Event.Subscribe<AnimationEnded>(AnimationEnded, this);
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
            Event.Subscribe<LevelSetup>(OnLevelSetup, this);
        }

        private void AnimationEnded(AnimationEnded obj)
        {
            if(--_animationsPending == 0)
            {
                if (_gameData.CurrentPhase == Phase.ResolvingCards)
                {
                    if (_gameData.CowboyState.HP <= 0 || _gameData.HouseState.HP <= 0)
                        _currentLevel++;
                    if (_gameData.CowboyState.HP <= 0)
                        Event.Publish(new PlayerDefeated { LevelNumber = _currentLevel, Winner = Player.House, IsGameOver = true });
                    else if (_gameData.HouseState.HP <= 0)
                        Event.Publish(new PlayerDefeated { LevelNumber = _currentLevel, Winner = Player.Cowboy, IsGameOver = false });
                    else
                    {
                        Event.Publish(new TurnFinished { TurnNumber = _gameData.CurrentTurn });
                        OnTurnFinished();
                    }
                }
                else if (_gameData.CurrentPhase == Phase.StartingTurn)
                {
                    OnTurnStarted();
                }
            }
        }

        private void OnLevelSetup(LevelSetup e)
        {
            _gameData.CurrentLevel = e.CurrentLevel;
            _gameData.CurrentTurn = 0;
            _currentLevel = e.CurrentLevel;
            Event.Publish(new TurnStarted { TurnNumber = _gameData.CurrentTurn });
            _gameData.CurrentPhase = Phase.StartingTurn;
            if (_animationsPending == 0)
                OnTurnStarted();
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            if (!e.IsGameOver)
                _gameData.CurrentPhase = Phase.LeavingLevel;
        }

        private void OnAllCardsSelected()
        {
            _gameData.CurrentPhase = Phase.ResolvingCards;
            if (_animationsPending == 0)
            {
                if (_gameData.CowboyState.HP <= 0 || _gameData.HouseState.HP <= 0)
                    _currentLevel++;
                if (_gameData.CowboyState.HP <= 0)
                    Event.Publish(new PlayerDefeated { LevelNumber = _currentLevel, Winner = Player.House, IsGameOver = true });
                else if (_gameData.HouseState.HP <= 0)
                    Event.Publish(new PlayerDefeated { LevelNumber = _currentLevel, Winner = Player.Cowboy, IsGameOver = false });
                else
                {
                    Event.Publish(new TurnFinished { TurnNumber = _gameData.CurrentTurn });
                    OnTurnFinished();
                }
            }
        }

        private void OnTurnFinished()
        {
            Event.Publish(new TurnStarted { TurnNumber = ++_gameData.CurrentTurn });
            _gameData.CurrentPhase = Phase.StartingTurn;
            if (_animationsPending == 0)
                OnTurnStarted();
        }

        private void OnTurnStarted()
        {
            _gameData.CurrentPhase = Phase.SelectingCards;
            Event.Publish(new DataStabilized(_gameData));
        }
        
        public void Update(TimeSpan delta)
        {
            if (_gameData.CurrentPhase == Phase.Setup)
            {
                _gameData.CurrentPhase = Phase.SelectingCards;
                Event.Publish(new NextLevelRequested() { Level = 1 });
            }
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
                OnAllCardsSelected();
            }
        }
    }
}
