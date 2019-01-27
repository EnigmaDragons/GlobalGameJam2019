using MonoDragons.GGJ.Gameplay;
using System.Collections.Generic;
using MonoDragons.Core.EventSystem;
using System;
using System.Linq;
using MonoDragons.GGJ.Gameplay.Events;

namespace MonoDragons.GGJ.Data
{
    public sealed class Cards
    {
        private static Dictionary<CardName, Func<CardState, CardView>> _cards = new Dictionary<CardName, Func<CardState, CardView>> {
            { CardName.None, s => new CardView(s, "CowboyCard0") },

            { CardName.CowboyPass, s => new CardView(s,"CowboyCard0") },
            { CardName.SixShooter, s => new CardView(s, "CowboyCard1") },
            { CardName.FanTheHammer, s => new CardView(s, "CowboyCard2") },
            { CardName.GunsBlazing, s => new CardView(s, "CowboyCard3") },
            { CardName.DuckAndCover, s => new CardView(s, "CowboyCard4") },
            { CardName.ShowDown, s => new CardView(s, "CowboyCard5") },
            { CardName.RushTheEnemy, s => new CardView(s, "CowboyCard6") },
            { CardName.LightTheFuse, s => new CardView(s, "CowboyCard7") },
            { CardName.Barricade, s => new CardView(s, "CowboyCard8") },
            { CardName.QuickDraw, s => new CardView(s, "CowboyCard9")  },
            { CardName.Lasso, s => new CardView(s, "CowboyCard10")  },
            { CardName.Ricochet, s => new CardView(s, "CowboyCard11")  },
            { CardName.BothBarrels, s => new CardView(s, "CowboyCard12")  },
            { CardName.CrackShot, s => new CardView(s, "CowboyCard13") },

            { CardName.HousePass, s => new CardView(s, "SmartHouseCard0") },
            { CardName.ElectricShockSuperAttack, s => new CardView(s, "SmartHouseCard1") },
            { CardName.WaterLeak, s => new CardView(s, "SmartHouseCard2") },
            { CardName.Lazer, s => new CardView(s, "SmartHouseCard3") }
        };

        private static Dictionary<CardName, CardType> _cardTypes = new Dictionary<CardName, CardType>
        {
            { CardName.None, CardType.Pass },

            { CardName.CowboyPass, CardType.Pass },
            { CardName.SixShooter, CardType.Attack },
            { CardName.FanTheHammer, CardType.Attack },
            { CardName.GunsBlazing, CardType.Attack },
            { CardName.DuckAndCover, CardType.Defend },
            { CardName.ShowDown, CardType.Charge },
            { CardName.RushTheEnemy, CardType.Attack },
            { CardName.LightTheFuse, CardType.Charge },
            { CardName.Barricade, CardType.Defend },
            { CardName.QuickDraw, CardType.Counter },
            { CardName.Lasso, CardType.Counter },
            { CardName.Ricochet, CardType.Counter },
            { CardName.BothBarrels, CardType.Attack },
            { CardName.CrackShot, CardType.Attack },

            { CardName.HousePass, CardType.Pass },
            { CardName.ElectricShockSuperAttack, CardType.Attack },
            { CardName.WaterLeak, CardType.Attack },
            { CardName.Lazer, CardType.Attack }
        };

        private static Dictionary<CardName, Action<GameData>> _cardActions = new Dictionary<CardName, Action<GameData>>
        {
            { CardName.None, data => {} },

            { CardName.CowboyPass, data => {} },
            { CardName.SixShooter, data => Event.Publish(new PlayerDamageProposed { Amount = 3, Target = Player.House }) },
            { CardName.FanTheHammer, data =>
                {
                    Event.Publish(new PlayerDamageProposed { Amount = 4, Target = Player.House });
                    Event.Publish(new CardTypeLocked { Target = Player.Cowboy, Type = CardType.Attack });
                } },
            { CardName.GunsBlazing, data =>
                {
                    Event.Publish(new PlayerDamageProposed { Amount = 4, Target = Player.House });
                    Event.Publish(new CardTypeLocked { Target = Player.Cowboy, Type = CardType.Defend });
                } },
            { CardName.DuckAndCover, data => Event.Publish(new PlayerBlockProposed { Amount = 5, Target = Player.Cowboy }) },
            { CardName.ShowDown, data => Event.Publish(new OnNotDamagedEffectQueued { Event = new NextAttackEmpowered { Target = Player.Cowboy, Amount = 6 } }) },
            { CardName.RushTheEnemy, data =>
                {
                    Event.Publish(new DamageTakenMultiplied { Target = Player.Cowboy, Multiplier = 2 });
                    Event.Publish(new PlayerDamageProposed { Target = Player.House, Amount = 5 });
                } },
            { CardName.LightTheFuse, data =>
                {
                    Event.Publish(new NextTurnEffectQueued
                    {
                        Event = new OnDamageEffectQueued
                        {
                            Target = Player.House,
                            Event = new PlayerDamaged {Target = Player.House, Amount = 6}
                        }
                    });
                    Event.Publish(new NextTurnEffectQueued
                    {
                        Event = new OnNotDamagedEffectQueued
                        {
                            Target = Player.House,
                            Event = new PlayerDamaged {Target = Player.Cowboy, Amount = 6}
                        }
                    });
                } },
            { CardName.Barricade, data =>
                {
                    Event.Publish(new PlayerBlockProposed { Target = Player.Cowboy, Amount = 3 });
                    Event.Publish(new NextTurnEffectQueued { Event = new PlayerBlockProposed { Target = Player.Cowboy, Amount = 3 } });
                } },
            { CardName.QuickDraw, data =>
                {
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Attack,
                        Event = new DamageTakenMultiplied { Target = Player.Cowboy, Multiplier = 0 } });
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Attack,
                        Event = new PlayerDamageProposed { Target = Player.House, Amount = 3 } });
                }},
            { CardName.Lasso, data =>
                {
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Charge,
                        Event = new NextTurnEffectQueued { Event = new DamageTakenMultiplied { Target = Player.House, Multiplier = 2 }} });
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Charge,
                        Event = new CardTypeLocked { Target = Player.House, Type = CardType.Attack } });
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Charge,
                        Event = new CardTypeLocked { Target = Player.House, Type = CardType.Defend } });
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Charge,
                        Event = new CardTypeLocked { Target = Player.House, Type = CardType.Charge } });
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Charge,
                        Event = new CardTypeLocked { Target = Player.House, Type = CardType.Counter } });
                } },
            { CardName.Ricochet, data =>
                {
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Defend,
                        Event = new BlockRecievedMultiplied { Target = Player.House, Multiplier = 0 } });
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Defend,
                        Event = new PlayerDamageProposed { Target = Player.House, Amount = 8 } });
                } },
            { CardName.BothBarrels, data =>
                {
                    Event.Publish(new PlayerDamageProposed { Target = Player.House, Amount = 2 });
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Attack,
                        Event = new PlayerDamageProposed { Target = Player.House, Amount = 2 }});
                } },
            { CardName.CrackShot, data =>
                {
                    Event.Publish(new PlayerDamageProposed { Target = Player.House, Amount = 2 });
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Charge,
                        Event = new PlayerDamageProposed { Target = Player.House, Amount = 3 }});
                } },

            { CardName.HousePass, data => {} },
            { CardName.ElectricShockSuperAttack, data => Event.Publish(new PlayerDamageProposed { Amount = 3, Target = Player.Cowboy }) },
            { CardName.WaterLeak, data => Event.Publish(new PlayerDamageProposed { Amount = 3, Target = Player.Cowboy }) },
            { CardName.Lazer, data => Event.Publish(new PlayerDamageProposed { Amount = 3, Target = Player.Cowboy }) },
        };

        public static CardView Create(CardState s)
        {
            s.Type = _cardTypes[s.CardName];
            return _cards[s.CardName](s);
        }

        public static void Execute(GameData data, CardName name)
        {
            _cardActions[name](data);
        }
    }

    public enum CardName
    {
        None = 0,

        CowboyPass = 1,
        SixShooter = 2,
        FanTheHammer = 3,
        GunsBlazing = 4,
        DuckAndCover = 9,
        ShowDown = 10,
        RushTheEnemy = 11,
        LightTheFuse = 12,
        Barricade = 13,
        QuickDraw = 14,
        Lasso = 15,
        Ricochet = 16,
        BothBarrels = 17,
        CrackShot = 18,

        HousePass = 5,
        ElectricShockSuperAttack = 6,
        WaterLeak = 7,
        Lazer = 8
    }
}
