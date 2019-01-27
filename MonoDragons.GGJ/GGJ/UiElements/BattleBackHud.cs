using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.UserInterface;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoDragons.GGJ.UiElements
{
    class BattleBackHud : IVisual
    {
        private readonly List<IVisual> _visuals;

        public BattleBackHud(Player player)
        {
            _visuals = new List<IVisual>
            {
                new Sprite{ Image= $"House/floor-{player.ToString().ToLower()}", Transform = new Transform2(UI.OfScreenSize(1.0f, 1.0f))} 
            };
        }

        public void Draw(Transform2 parentTransform)
        {
            _visuals.ForEach(x => x.Draw(new Transform2(new Vector2(0, 26))));
        }
    }
}
