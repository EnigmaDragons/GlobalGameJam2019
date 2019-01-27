using MonoDragons.GGJ.Gameplay;
using System.Collections.Generic;
using MonoDragons.Core.EventSystem;
using System;
using MonoDragons.GGJ.Gameplay.Events;
using System.Linq;
using MonoDragons.GGJ.UiElements;

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
            { CardName.Reload, s => new CardView(s, "CowboyCard14") },
            { CardName.Wanted, s => new CardView(s, "CowboyCard15") },

            { CardName.HousePass, s => new CardView(s, "SmartHouseCard0") },
            { CardName.Lamp, s => new CardView(s, "SmartHouseCard1") },
            { CardName.LightsOut, s => new CardView(s, "SmartHouseCard2") },
            { CardName.BlindingLights, s => new CardView(s, "SmartHouseCard3") },
            { CardName.DustTheRoom, s => new CardView(s, "SmartHouseCard4") },
            { CardName.HeatUp, s => new CardView(s, "SmartHouseCard5") },
            { CardName.CoolDown, s => new CardView(s, "SmartHouseCard6") },
            { CardName.ShippingBoxesWall, s => new CardView(s, "SmartHouseCard7") },
            { CardName.SpinningFanBlades, s => new CardView(s, "SmartHouseCard8") },
            { CardName.RoombaAttack, s => new CardView(s, "SmartHouseCard9") },
            { CardName.PowerCordTrip, s => new CardView(s, "SmartHouseCard10") },

            { CardName.BedderLuckNextTime, s => new CardView(s, "SmartHouseCard11") },
            { CardName.PillowFight, s => new CardView(s, "SmartHouseCard12") },
            { CardName.Resting, s => new CardView(s, "SmartHouseCard13") },
            { CardName.PillowFort, s => new CardView(s, "SmartHouseCard14") },
            { CardName.ThatsCurtainsForYou, s => new CardView(s, "SmartHouseCard15") },
            { CardName.MonsterUnderTheBed, s => new CardView(s, "SmartHouseCard16") },

            { CardName.HologramProjection, s => new CardView(s, "SmartHouseCard17") },
            { CardName.InformationOverload, s => new CardView(s, "SmartHouseCard18") },
            { CardName.DeathTrap, s => new CardView(s, "SmartHouseCard19") },
            { CardName.HammerDownload, s => new CardView(s, "SmartHouseCard20") },
            { CardName.AdaptiveTactics, s => new CardView(s, "SmartHouseCard21") },
            { CardName.BoringWikiArticle, s => new CardView(s, "SmartHouseCard22") },
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
            { CardName.Reload, CardType.Defend },
            { CardName.Wanted, CardType.Charge },

            { CardName.HousePass, CardType.Pass },
            { CardName.Lamp, CardType.Attack },
            { CardName.LightsOut, CardType.Defend },
            { CardName.BlindingLights, CardType.Attack },
            { CardName.DustTheRoom, CardType.Attack },
            { CardName.HeatUp, CardType.Charge },
            { CardName.CoolDown, CardType.Charge },
            { CardName.ShippingBoxesWall, CardType.Defend },
            { CardName.SpinningFanBlades, CardType.Counter },
            { CardName.RoombaAttack, CardType.Counter },
            { CardName.PowerCordTrip, CardType.Counter },

            { CardName.BedderLuckNextTime, CardType.Attack },
            { CardName.PillowFight, CardType.Attack },
            { CardName.Resting, CardType.Charge },
            { CardName.PillowFort, CardType.Defend },
            { CardName.ThatsCurtainsForYou, CardType.Attack },
            { CardName.MonsterUnderTheBed, CardType.Attack },

            { CardName.HologramProjection, CardType.Defend },
            { CardName.InformationOverload, CardType.Attack },
            { CardName.DeathTrap, CardType.Counter },
            { CardName.HammerDownload, CardType.Attack },
            { CardName.AdaptiveTactics, CardType.Charge },
            { CardName.BoringWikiArticle, CardType.Attack }
        };

        private static Dictionary<CardName, int> _attackMap = new Dictionary<CardName, int>
        {
            { CardName.None, 0 },

            { CardName.CowboyPass, 0 },
            { CardName.SixShooter, 3 },
            { CardName.FanTheHammer, 4 },
            { CardName.GunsBlazing, 4 },
            { CardName.DuckAndCover, 0 },
            { CardName.ShowDown, 0 },
            { CardName.RushTheEnemy, 5 },
            { CardName.LightTheFuse, 0 },
            { CardName.Barricade, 0 },
            { CardName.QuickDraw, 3 },
            { CardName.Lasso, 0 },
            { CardName.Ricochet, 8 },
            { CardName.BothBarrels, 2 },
            { CardName.CrackShot, 2 },
            { CardName.Reload, 0 },
            { CardName.Wanted, 0 },

            { CardName.HousePass, 0 },
            { CardName.Lamp, 3 },
            { CardName.LightsOut, 0 },
            { CardName.BlindingLights, 2 },
            { CardName.DustTheRoom, 2 },
            { CardName.HeatUp, 2 },
            { CardName.CoolDown, 0 },
            { CardName.ShippingBoxesWall, 0 },
            { CardName.SpinningFanBlades, 12 },
            { CardName.RoombaAttack, 6 },
            { CardName.PowerCordTrip, 3 },

            { CardName.BedderLuckNextTime, 0 },
            { CardName.PillowFight, 2 },
            { CardName.Resting, 0 },
            { CardName.PillowFort, 0 },
            { CardName.ThatsCurtainsForYou, 1 },
            { CardName.MonsterUnderTheBed, 1 },

            { CardName.HologramProjection, 0 },
            { CardName.InformationOverload, 4 },
            { CardName.DeathTrap, 10 },
            { CardName.HammerDownload, 2 },
            { CardName.AdaptiveTactics, 0 },
            { CardName.BoringWikiArticle, 3 }
        };

        private static Dictionary<CardName, int> _defenseMap = new Dictionary<CardName, int>
        {
            { CardName.None, 0 },

            { CardName.CowboyPass, 0 },
            { CardName.SixShooter, 0 },
            { CardName.FanTheHammer, 0 },
            { CardName.GunsBlazing, 0 },
            { CardName.DuckAndCover, 5 },
            { CardName.ShowDown, 0 },
            { CardName.RushTheEnemy, 0 },
            { CardName.LightTheFuse, 0 },
            { CardName.Barricade, 3 },
            { CardName.QuickDraw, 0 },
            { CardName.Lasso, 0 },
            { CardName.Ricochet, 0 },
            { CardName.BothBarrels, 0 },
            { CardName.CrackShot, 0 },
            { CardName.Reload, 2 },
            { CardName.Wanted, 0 },

            { CardName.HousePass, 0 },
            { CardName.Lamp, 0 },
            { CardName.LightsOut, 3 },
            { CardName.BlindingLights, 0 },
            { CardName.DustTheRoom, 0 },
            { CardName.HeatUp, 0 },
            { CardName.CoolDown, 0 },
            { CardName.ShippingBoxesWall, 5 },
            { CardName.SpinningFanBlades, 0 },
            { CardName.RoombaAttack, 0 },
            { CardName.PowerCordTrip, 0 },

            { CardName.BedderLuckNextTime, 0 },
            { CardName.PillowFight, 0 },
            { CardName.Resting, 0 },
            { CardName.PillowFort, 2 },
            { CardName.ThatsCurtainsForYou, 0 },
            { CardName.MonsterUnderTheBed, 0 },

            { CardName.HologramProjection, 0 },
            { CardName.InformationOverload, 0 },
            { CardName.DeathTrap, 0 },
            { CardName.HammerDownload, 0 },
            { CardName.AdaptiveTactics, 0 },
            { CardName.BoringWikiArticle, 0 }
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
            { CardName.ShowDown, data => Event.Publish(new NotDamagedEffectQueued { Event = new NextAttackEmpowered { Target = Player.Cowboy, Amount = 6 } }) },
            { CardName.RushTheEnemy, data =>
                {
                    Event.Publish(new DamageTakenMultiplied { Target = Player.Cowboy, Type = MultiplierType.Double });
                    Event.Publish(new PlayerDamageProposed { Target = Player.House, Amount = 5 });
                } },
            { CardName.LightTheFuse, data =>
                {
                    Event.Publish(new NextTurnEffectQueued
                    {
                        Event = new DamageEffectQueued
                        {
                            Target = Player.House,
                            Event = new PlayerDamaged {Target = Player.House, Amount = 6}
                        }
                    });
                    Event.Publish(new NextTurnEffectQueued
                    {
                        Event = new NotDamagedEffectQueued
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
                        Event = new DamageTakenMultiplied { Target = Player.Cowboy, Type = MultiplierType.Zero } });
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Attack,
                        Event = new PlayerDamageProposed { Target = Player.House, Amount = 3 } });
                }},
            { CardName.Lasso, data =>
                {
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Charge,
                        Event = new NextTurnEffectQueued { Event = new DamageTakenMultiplied { Target = Player.House, Type = MultiplierType.Double }} });
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
                        Event = new BlockRecievedMultiplied { Target = Player.House, Type = MultiplierType.Zero } });
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
            { CardName.Reload, data =>
                {
                    Event.Publish(new PlayerBlockProposed { Target = Player.Cowboy, Amount = 2 });
                    Event.Publish(new DamageNotBlockedEffectQueued { Target = Player.Cowboy,
                        Event = new NextAttackEmpowered { Target = Player.Cowboy, Amount = 4 } });
                } },
            { CardName.Wanted, data =>
                {
                    Event.Publish(new NextAttackEmpowered { Target = Player.Cowboy, Amount = 2 });
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Attack,
                        Event = new DamageTakenMultiplied { Target = Player.Cowboy, Type = MultiplierType.Half }});
                    Event.Publish(new CounterEffectQueued { Caster = Player.Cowboy, Type = CardType.Attack,
                        Event = new BlockRecievedMultiplied { Target = Player.House, Type = MultiplierType.Half }});
                } },

            { CardName.HousePass, data => {} },
            { CardName.Lamp, data => Event.Publish(new PlayerDamageProposed { Amount = 3, Target = Player.Cowboy }) },
            { CardName.LightsOut, data =>
                {
                    Event.Publish(new PlayerBlockProposed { Amount = 3, Target = Player.House } );
                    Event.Publish(new StatusApplied { Target = Player.Cowboy, Status = new Status { Name = "Darkness", Events = new List<object> { new NextTurnEffectQueued { Event = new StatusRemoved { Name = "Darkness", Target = Player.Cowboy } } } }});
                    if (data.CowboyState.Statuses.Any(x => x.Name == "Lightness"))
                    {
                        Event.Publish(new PlayerBlockProposed { Amount = 6, Target = Player.House });
                        Event.Publish(new NextAttackEmpowered { Amount = 3, Target = Player.House });
                    }
                }
            },
            { CardName.BlindingLights, data =>
                {
                    Event.Publish(new PlayerDamageProposed { Amount = 1, Target = Player.Cowboy });
                    Event.Publish(new PlayerBlockProposed { Amount = 1, Target = Player.House } );
                    Event.Publish(new StatusApplied { Target = Player.Cowboy, Status = new Status { Name = "Lightness", Events = new List<object> { new NextTurnEffectQueued { Event = new StatusRemoved { Name = "Lightness", Target = Player.Cowboy } } } }});
                    if (data.CowboyState.Statuses.Any(x => x.Name == "Darkness"))
                    {
                        Event.Publish(new PlayerDamageProposed { Amount = 2, Target = Player.Cowboy });
                        Event.Publish(new PlayerBlockProposed { Amount = 2, Target = Player.House } );
                        Event.Publish(new NextAttackEmpowered { Amount = 3, Target = Player.House });
                    }
                }
            },
            { CardName.DustTheRoom, data =>
                {
                    Event.Publish(new PlayerDamageProposed { Target = Player.Cowboy, Amount = 2 });
                    Event.Publish(new NextTurnEffectQueued { Event = new PlayerDamageProposed { Target = Player.Cowboy, Amount = 2 }});
                } },
            { CardName.HeatUp, data =>
                {
                    Event.Publish(new PlayerDamageProposed { Target = Player.Cowboy, Amount = 2 });
                    Event.Publish(new PlayerDamageProposed { Target = Player.House, Amount = 2 });
                    Event.Publish(new NextAttackEmpowered { Target = Player.House, Amount = 2 });
                    Event.Publish(new DamageEffectQueued { Target = Player.House, Event = new NextAttackEmpowered { Target = Player.House, Amount = 4 }});
                } },
            { CardName.CoolDown, data =>
                {
                    Event.Publish(new NextAttackEmpowered { Target = Player.House, Amount = 3 });
                    Event.Publish(new NextTurnEffectQueued { Event = new DamageTakenMultiplied { Target = Player.Cowboy, Type = MultiplierType.Half }});
                } },
            { CardName.ShippingBoxesWall, data => Event.Publish(new PlayerBlockProposed { Amount = 5, Target = Player.House }) },
            { CardName.SpinningFanBlades, data =>
                {
                    Event.Publish(new CounterEffectQueued { Caster = Player.House, Type = CardType.Charge,
                        Event = new PlayerDamageProposed { Target = Player.Cowboy, Amount = 12 } });
                } },
            { CardName.RoombaAttack, data =>
                {
                    Event.Publish(new CounterEffectQueued { Caster = Player.House, Type = CardType.Defend,
                        Event = new BlockRecievedMultiplied { Target = Player.Cowboy, Type = MultiplierType.Zero } });
                    Event.Publish(new CounterEffectQueued { Caster = Player.House, Type = CardType.Defend,
                        Event = new CardTypeLocked { Target = Player.Cowboy, Type = CardType.Defend }});
                    Event.Publish(new CounterEffectQueued { Caster = Player.House, Type = CardType.Defend,
                        Event = new PlayerDamageProposed { Target = Player.Cowboy, Amount = 6 } });
                } },
            { CardName.PowerCordTrip, data =>
                {
                    Event.Publish(new CounterEffectQueued { Caster = Player.House, Type = CardType.Attack,
                        Event = new PlayerDamageProposed { Target = Player.Cowboy, Amount = 3 }});
                    Event.Publish(new CounterEffectQueued { Caster = Player.House, Type = CardType.Attack,
                        Event = new CardTypeLocked { Target = Player.Cowboy, Type = CardType.Attack }});
                    Event.Publish(new CounterEffectQueued { Caster = Player.House, Type = CardType.Attack,
                        Event = new CardTypeLocked { Target = Player.Cowboy, Type = CardType.Defend }});
                    Event.Publish(new CounterEffectQueued { Caster = Player.House, Type = CardType.Attack,
                        Event = new CardTypeLocked { Target = Player.Cowboy, Type = CardType.Charge }});
                    Event.Publish(new CounterEffectQueued { Caster = Player.House, Type = CardType.Attack,
                        Event = new CardTypeLocked { Target = Player.Cowboy, Type = CardType.Counter }});
                } },

            { CardName.BedderLuckNextTime, data =>
                {
                    Event.Publish(new EnergyGained { Amount = 1, Target = Player.House });
                    Event.Publish(new PlayerDamageProposed { Target = Player.Cowboy, Amount = data.HouseState.Energy * 2 });
                    Event.Publish(new EnergyLossed { Target = Player.House, Amount = data.HouseState.Energy });
                } },
            { CardName.PillowFight, data =>
                {
                    Event.Publish(new EnergyGained { Amount = 1, Target = Player.House });
                    Event.Publish(new PlayerDamageProposed { Target = Player.Cowboy, Amount = 2 });
                } },
            { CardName.Resting, data =>
                {
                    Event.Publish(new EnergyGained { Amount = 3, Target = Player.House });
                } },
            { CardName.PillowFort, data =>
                {
                    Event.Publish(new EnergyGained { Amount = 1, Target = Player.House });
                    Event.Publish(new PlayerBlockProposed { Target = Player.House, Amount = 3 });
                } },
            { CardName.ThatsCurtainsForYou, data =>
                {
                    Event.Publish(new EnergyGained { Amount = 1, Target = Player.House });
                    Event.Publish(new PlayerDamageProposed { Target = Player.Cowboy, Amount = 1 });
                    Event.Publish(new CounterEffectQueued { Caster = Player.House, Type = CardType.Attack,
                        Event = new PlayerDamageProposed { Target = Player.Cowboy, Amount = 2 }});
                } },
            { CardName.MonsterUnderTheBed, data =>
                {
                    Event.Publish(new EnergyGained { Amount = 1, Target = Player.House });
                    Event.Publish(new PlayerDamageProposed { Target = Player.Cowboy, Amount = 1 });
                    Event.Publish(new CounterEffectQueued { Caster = Player.House, Type = CardType.Charge,
                        Event = new PlayerDamageProposed { Target = Player.Cowboy, Amount = 4 }});
                } },

            { CardName.HologramProjection, data =>
                {
                    Event.Publish(new DamageTakenMultiplied { Target = Player.House, Type = MultiplierType.Zero });
                    Event.Publish(new CardTypeLocked { Target = Player.House, Type = CardType.Defend });
                    Event.Publish(new HandSizeAdjusted { Target = Player.Cowboy, Adjustment = 1 });
                } },
            { CardName.InformationOverload, data =>
                {
                    Event.Publish(new PlayerDamageProposed { Target = Player.Cowboy, Amount = 4 });
                    Event.Publish(new HandSizeAdjusted { Target = Player.Cowboy, Adjustment = 1 });
                } },
            { CardName.DeathTrap, data =>
                {
                    Event.Publish(new CounterEffectQueued { Caster = Player.House, Type = CardType.Counter,
                        Event = new PlayerDamageProposed {Amount = 10, Target = Player.Cowboy}});
                    Event.Publish(new CounterEffectQueued { Caster = Player.House, Type = CardType.Counter,
                        Event = new HandSizeAdjusted { Adjustment = -2, Target = Player.Cowboy }});
                } },
            { CardName.HammerDownload, data =>
                {
                    Event.Publish(new PlayerDamageProposed { Target = Player.Cowboy, Amount = 2 });
                    Event.Publish(new HandSizeAdjusted { Adjustment = 2, Target = Player.House });
                } },
            { CardName.AdaptiveTactics, data =>
                {
                    Event.Publish(new PlayerBlockProposed { Target = Player.House, Amount = 3 });
                    Event.Publish(new HandSizeAdjusted { Target = Player.Cowboy, Adjustment = 1 });
                    Event.Publish(new LastPlayedTypeLocked { Target = Player.Cowboy });
                    Event.Publish(new StatusApplied { Target = Player.Cowboy, Status = new Status { Name = "Analyze Tactics", Events = new List<object> { new HandSizeAdjusted { Target = Player.Cowboy, Adjustment = 1 }, new LastPlayedTypeLocked { Target = Player.Cowboy }}}});
                } },
            { CardName.BoringWikiArticle, data =>
                {
                    Event.Publish(new PlayerDamageProposed { Target = Player.Cowboy, Amount = 3 });
                    Event.Publish(new HandSizeAdjusted { Adjustment = 1, Target = Player.House });
                    Event.Publish(new HandSizeAdjusted { Adjustment = 1, Target = Player.Cowboy });
                } },
        };

        public static CardView Create(CardState s)
        {
            s.Type = _cardTypes[s.CardName];
            s.PredictedDamage = _attackMap[s.CardName];
            s.PredictedBlock = _defenseMap[s.CardName];
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
        Reload = 19,

        HousePass = 5,
        Lamp = 6,
        LightsOut = 7,
        BlindingLights = 8,
        DustTheRoom = 20,
        HeatUp = 21,
        CoolDown = 22,
        ShippingBoxesWall = 23,
        SpinningFanBlades = 24,
        RoombaAttack = 25,
        PowerCordTrip = 26,
        Wanted = 39,

        BedderLuckNextTime = 27,
        PillowFight = 28,
        Resting = 29,
        PillowFort = 30,
        ThatsCurtainsForYou = 31,
        MonsterUnderTheBed = 32,

        HologramProjection = 33,
        InformationOverload = 34,
        DeathTrap = 35,
        HammerDownload = 36,
        AdaptiveTactics = 37,
        BoringWikiArticle = 38
    }
}
