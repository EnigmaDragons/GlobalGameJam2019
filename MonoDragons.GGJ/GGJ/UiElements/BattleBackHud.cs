using System;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.UserInterface;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoDragons.GGJ.UiElements
{
    class BattleBackHud : IVisualAutomaton
    {
        private readonly GameData _data;
        private static readonly Size2 HpSize = new Size2(68, 68);
        private readonly Label _cowboyHp = new Label { Transform = new Transform2(UI.OfScreen(0.008f, 0.78f), HpSize) };
        private readonly Label _houseHp = new Label { Transform = new Transform2(UI.OfScreen(0.946f, 0.78f), HpSize) };
        private readonly Label _cowboyNrg = new Label { Transform = new Transform2(UI.OfScreen(0.008f, 0.88f), HpSize), TextColor = UiConsts.DarkBrown };
        private readonly Label _houseNrg = new Label { Transform = new Transform2(UI.OfScreen(0.946f, 0.88f), HpSize), TextColor = UiConsts.DarkBrown };
        private readonly List<IVisual> _visuals;

        public BattleBackHud(Player player, GameData data)
        {
            _data = data;
            _visuals = new List<IVisual>
            {
                new Sprite{ Image= $"House/floor-{player.ToString().ToLower()}", Transform = new Transform2(new Vector2(0, 26), UI.OfScreenSize(1.0f, 1.0f))} ,
                new UiImage { Image= "UI/heart", Transform = new Transform2(UI.OfScreen(0.008f, 0.78f), HpSize)},
                new UiImage { Image= "UI/heart", Transform = new Transform2(UI.OfScreen(0.946f, 0.78f), HpSize)},
                _cowboyHp,
                _houseHp,
                new UiImage { Image= "UI/energy", Transform = new Transform2(UI.OfScreen(0.008f, 0.88f), HpSize)},
                new UiImage { Image= "UI/energy", Transform = new Transform2(UI.OfScreen(0.946f, 0.88f), HpSize)},
                _cowboyNrg,
                _houseNrg,
            };
        }

        public void Draw(Transform2 parentTransform)
        {
            _visuals.ForEach(x => x.Draw(parentTransform));
        }
        
        public void Update(TimeSpan delta)
        {
            if (!_data.IsInitialized)
                return;
            
            _cowboyHp.Text = _data.CowboyState.HP.ToString();
            _houseHp.Text = _data.HouseState.HP.ToString();
            _cowboyNrg.Text = _data.CowboyState.Energy.ToString();
            _houseNrg.Text = _data.HouseState.Energy.ToString();
        }
    }
}
