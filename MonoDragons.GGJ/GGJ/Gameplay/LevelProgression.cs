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
                new CharacterState(Player.House, 10,
                    new PlayerCardsState(
                        CreateCard(CardName.HousePass),
                        CreateCard(CardName.Lamp),
                        CreateCard(CardName.LightsOut),
                        CreateCard(CardName.BlindingLights),
                        CreateCard(CardName.DustTheRoom),
                        CreateCard(CardName.HeatUp),
                        CreateCard(CardName.CoolDown),
                        CreateCard(CardName.ShippingBoxesWall),
                        CreateCard(CardName.SpinningFanBlades),
                        CreateCard(CardName.RoombaAttack),
                        CreateCard(CardName.PowerCordTrip),
                        
                        CreateCard(CardName.BedderLuckNextTime),
                        CreateCard(CardName.PillowFight),
                        CreateCard(CardName.Resting),
                        CreateCard(CardName.PillowFort),
                        CreateCard(CardName.ThatsCurtainsForYou),
                        CreateCard(CardName.MonsterUnderTheBed))));

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