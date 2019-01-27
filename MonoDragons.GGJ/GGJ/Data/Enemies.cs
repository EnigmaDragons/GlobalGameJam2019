using System;
using System.Collections.Generic;
using System.Linq;
using MonoDragons.GGJ.Gameplay;

namespace MonoDragons.GGJ.Data
{
    public enum Enemy
    {
        Bed,
        Computer,
    }
    
    public static class Enemies
    {      
        private static readonly Dictionary<Enemy, Func<GameData, List<CardState>>> _enemySpecificCards = new Dictionary<Enemy, Func<GameData, List<CardState>>>
        {
            { Enemy.Bed, data => new List<CardState>
                {
                    CreateCard(data, CardName.BedderLuckNextTime),
                    CreateCard(data, CardName.PillowFight),
                    CreateCard(data, CardName.Resting),
                    CreateCard(data, CardName.PillowFort),
                    CreateCard(data, CardName.ThatsCurtainsForYou),
                    CreateCard(data, CardName.MonsterUnderTheBed)
                } },
            { Enemy.Computer, data => new List<CardState>
                {
                    CreateCard(data, CardName.HologramProjection),
                    CreateCard(data, CardName.InformationOverload),
                    CreateCard(data, CardName.DeathTrap),
                    CreateCard(data, CardName.HammerDownload),
                    CreateCard(data, CardName.AdaptiveTactics),
                    CreateCard(data, CardName.BoringWikiArticle)
                } },
        };

        public static IHouseChar Create(Enemy enemy)
        {
            if (enemy == Enemy.Bed)
                return new Bed();
            if (enemy == Enemy.Computer)
                return new Computer();
            throw new KeyNotFoundException($"Unknown Enemy: {enemy}");
        }

        public static List<CardState> CreateEnemyDeck(GameData data, Enemy enemy)
        {
            return new List<CardState>
            {
                CreateCard(data, CardName.HousePass),
                CreateCard(data, CardName.Lamp),
                CreateCard(data, CardName.LightsOut),
                CreateCard(data, CardName.BlindingLights),
                CreateCard(data, CardName.DustTheRoom),
                CreateCard(data, CardName.HeatUp),
                CreateCard(data, CardName.CoolDown),
                CreateCard(data, CardName.ShippingBoxesWall),
                CreateCard(data, CardName.SpinningFanBlades),
                CreateCard(data, CardName.RoombaAttack),
                CreateCard(data, CardName.PowerCordTrip),
            }.Concat(_enemySpecificCards[enemy](data)).ToList();
        }

        private static CardState CreateCard(GameData data, CardName cardName)
        {
            var result = new CardState { Id = GetNextCardId(data), CardName = cardName };
            data.AllCards[result.Id] = result;
            return result;
        }

        private static int GetNextCardId(GameData data)
        {
            var result = data.CurrentCardId;
            data.CurrentCardId++;
            return result;
        }
    }
}