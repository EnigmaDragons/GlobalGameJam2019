using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Data;

namespace MonoDragons.GGJ.Gameplay
{
    public sealed class LevelProgression
    {
        private readonly GameData _data;
        private readonly HouseCharacters _house;

        public LevelProgression(GameData data, HouseCharacters house)
        {
            _data = data;
            _house = house;
            Event.Subscribe<FinishedLevel>(OnLevelFinished, this);
            Event.Subscribe<NextLevelRequested>(x => SetupCharacters(x.Level), this);
        }
        
        private void SetupCharacters(int level)
        {
            _data.InitLevel(level, 
                new CharacterState(Player.Cowboy, 10,
                    new PlayerCardsState(
                        CreateCard(CardName.CowboyPass),
                        CreateCard(CardName.CrackShot),
                        CreateCard(CardName.FanTheHammer),
                        CreateCard(CardName.GunsBlazing),
                        CreateCard(CardName.ShowDown),
                        CreateCard(CardName.RushTheEnemy))),
                new CharacterState(Player.House, 10,
                    new PlayerCardsState(
                        CreateCard(CardName.HousePass),
                        CreateCard(CardName.Lazer),
                        CreateCard(CardName.WaterLeak),
                        CreateCard(CardName.ElectricShockSuperAttack))));

            _house.Initialized(Enemies.Create(Enemy.Bed));
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
            if (!e.IsGameOver && _data.CurrentLevel < e.LevelNumber)
            {
                Event.Publish(new NextLevelRequested { Level = _data.CurrentLevel + 1 });
                Logger.WriteLine("-----------------------------------------------------");
            }
        }
    }
}