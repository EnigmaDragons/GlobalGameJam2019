using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.UserInterface;
using System;
using System.Collections.Generic;

namespace MonoDragons.GGJ.UiElements
{
    class BattleTopHud : IVisualAutomaton
    {
        private readonly Label _cowboyHp = new Label { Transform = new Transform2(UI.OfScreen(0.038f, 0.02f), new Size2(92, 92)) };
        private readonly Label _houseHp = new Label { Transform = new Transform2(UI.OfScreen(0.9f, 0.02f), new Size2(92, 92)) };

        private IReadOnlyList<IVisual> _visuals = new List<IVisual>();
        private readonly MustInit<GameData> _g = new MustInit<GameData>("Game Data");

        public BattleTopHud Initialized(GameData g)
        {
            _g.Init(g);
            _cowboyHp.Text = _g.Get().CowboyState.HP.ToString();
            _houseHp.Text = _g.Get().HouseState.HP.ToString();
            _visuals = new List<IVisual>
            {
                new UiImage { Image= "UI/wood-box", Transform = new Transform2(UI.OfScreen(-0.04f, -0.14f), new Size2(300, 300))},
                new UiImage { Image= "UI/wood-box", Transform = new Transform2(UI.OfScreen(0.85f, -0.14f), new Size2(300, 300))},
                new UiImage { Image= "UI/heart", Transform = new Transform2(UI.OfScreen(0.038f, 0.02f), new Size2(92, 92))},
                new UiImage { Image= "UI/heart", Transform = new Transform2(UI.OfScreen(0.9f, 0.02f), new Size2(92, 92))},
                _cowboyHp,
                _houseHp,
            };
            return this;
        }

        public void Draw(Transform2 parentTransform)
        {
            _visuals.ForEach(x => x.Draw(parentTransform));            
        }

        public void Update(TimeSpan delta)
        {
            _cowboyHp.Text = _g.Get().CowboyState.HP.ToString();
            _houseHp.Text = _g.Get().HouseState.HP.ToString();
        }
    }
}
