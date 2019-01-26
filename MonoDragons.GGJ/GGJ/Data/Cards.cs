using MonoDragons.GGJ.Gameplay;
using System.Collections.Generic;
using MonoDragons.Core.EventSystem;
using System;

namespace MonoDragons.GGJ.Data
{
    public sealed class Cards
    {
        private static Dictionary<CardName, Func<CardState, Card>> _cards = new Dictionary<CardName, Func<CardState, Card>> {
            { CardName.None, s => new Card(s, "CowboyCard0", () => {}) },
            { CardName.CowboyPass, s => new Card(s,"CowboyCard0", () => {}) },
            { CardName.YEEHAW, s => new Card(s,"CowboyCard1", () => Event.Publish(new PlayerDamaged { Amount = 1, Target = Player.House })) },
            { CardName.SixShooterThingy, s => new Card(s, "CowboyCard2", () => Event.Publish(new PlayerDamaged { Amount = 1, Target = Player.House })) },
            { CardName.DeadEye, s => new Card(s, "CowboyCard3", () => Event.Publish(new PlayerDamaged { Amount = 1, Target = Player.House })) },
            { CardName.HousePass, s => new Card(s, "SmartHouseCard0", () => {}) },
            { CardName.ElectricShockSuperAttack, s => new Card(s, "SmartHouseCard1", () => Event.Publish(new PlayerDamaged { Amount = 1, Target = Player.Cowboy })) },
            { CardName.WaterLeak, s => new Card(s, "SmartHouseCard2", () => Event.Publish(new PlayerDamaged { Amount = 1, Target = Player.Cowboy })) },
            { CardName.Lazer, s => new Card(s, "SmartHouseCard3", () => Event.Publish(new PlayerDamaged { Amount = 1, Target = Player.Cowboy })) }
        };

        public static Card Create(CardState s)
        {
            return _cards[s.CardName](s);
        }
    }

    public enum CardName
    {
        None = 0,

        CowboyPass = 1,
        YEEHAW = 2,
        SixShooterThingy = 3,
        DeadEye = 4,

        HousePass = 5,
        ElectricShockSuperAttack = 6,
        WaterLeak = 7,
        Lazer = 8
    }
}
