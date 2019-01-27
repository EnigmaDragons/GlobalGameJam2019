using System.Collections.Generic;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Scenes;
using MonoDragons.GGJ.Data;
using MonoDragons.GGJ.Scenes;

namespace MonoDragons.GGJ.Gameplay
{
    public sealed class LevelProgression
    {
        private const int NumLevels = 2;
        private readonly GameData _data;
        private readonly HouseCharacters _house;
        private readonly List<Enemy> _enemyOrder;

        public LevelProgression(GameData data, HouseCharacters house)
        {
            _data = data;
            _house = house;
            _enemyOrder = new List<Enemy> { Enemy.Bed, Enemy.Computer };
            Event.Subscribe<FinishedLevel>(OnLevelFinished, this);
            Event.Subscribe<NextLevelRequested>(x => SetupCharacters(x.Level), this);
        }
        
        private void SetupCharacters(int level)
        {
            _data.InitLevel(level, 
                new CharacterState(Player.Cowboy, level == 1 ? 40 : _data.CowboyState.HP + 15,
                    new PlayerCardsState(
                        CreateCard(CardName.CowboyPass),
                        CreateCard(CardName.SixShooter),
                        CreateCard(CardName.FanTheHammer),
                        CreateCard(CardName.GunsBlazing),
                        CreateCard(CardName.ShowDown),
                        CreateCard(CardName.RushTheEnemy),
                        CreateCard(CardName.LightTheFuse),
                        CreateCard(CardName.Barricade),
                        CreateCard(CardName.QuickDraw),
                        CreateCard(CardName.Lasso),
                        CreateCard(CardName.Ricochet),
                        CreateCard(CardName.BothBarrels),
                        CreateCard(CardName.CrackShot),
                        CreateCard(CardName.Reload))),
                new CharacterState(Player.House, 20 + level * 5, new PlayerCardsState(Enemies.CreateEnemyDeck(_data, _enemyOrder[level - 1]))));
            _data.CurrentEnemy = _enemyOrder[level - 1];
            _house.Initialized(Enemies.Create(_enemyOrder[level - 1]));
        }

        private CardState CreateCard(CardName cardName)
        {
            var result = new CardState { Id = GetNextCardId(), CardName = cardName };
            _data.AllCards[result.Id] = result;
            return result;
        }

        private int GetNextCardId()
        {
            var result = _data.CurrentCardId;
            _data.CurrentCardId++;
            return result;
        }

        private void OnLevelFinished(FinishedLevel e)
        {
            if (e.LevelNumber > NumLevels)
            {
                Scene.NavigateTo(new CreditsScene(Player.Cowboy));
            }
            else if (!e.IsGameOver && _data.CurrentLevel < e.LevelNumber)
            {
                Event.Publish(new NextLevelRequested { Level = _data.CurrentLevel + 1 });
                Logger.WriteLine("-----------------------------------------------------");
            }
        }
    }
}