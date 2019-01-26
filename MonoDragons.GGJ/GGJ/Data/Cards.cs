using MonoDragons.GGJ.Gameplay;
using System.Collections.Generic;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Gameplay.Events;

namespace MonoDragons.GGJ.Data
{
    public class Cards
    {
        private static Dictionary<int, Card> _cards = new Dictionary<int, Card> {
            { 0, new Card("", 0 , () => {}) },
            { 1, new Card("CowboyCard0", 1, () => Event.Publish(new PlayerDamaged { Amount = 1, Target = Player.House })) },
            { 2, new Card("CowboyCard1", 2, () => Event.Publish(new PlayerDamaged { Amount = 1, Target = Player.House })) },
            { 3, new Card("CowboyCard2", 3, () => Event.Publish(new PlayerDamaged { Amount = 1, Target = Player.House })) },
            { 4, new Card("SmartHouseCards0", 4, () => Event.Publish(new PlayerDamaged { Amount = 1, Target = Player.Cowboy })) },
            { 5, new Card("SmartHouseCards1", 5, () => Event.Publish(new PlayerDamaged { Amount = 1, Target = Player.Cowboy })) },
            { 6, new Card("SmartHouseCards2", 6, () => Event.Publish(new PlayerDamaged { Amount = 1, Target = Player.Cowboy })) }
        };

        public static Card GetCardById(CardName name)
        {
            return GetCardById((int) name);
        }

        public static Card GetCardById(int id)
        {
            return _cards[id];
        }
    }

    public enum CardName
    {
        YEEHAW = 1,
        SixShooterThingy = 2,
        DeadEye = 3,
        ElectricShockSuperAttack = 4,
        WaterLeak = 5,
        Lazer = 6
    }
}
