using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Graphics;
using MonoDragons.Core.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.GGJ.Gameplay
{
    public class Card : IVisual
    {
        UiColoredRectangle cardImage = new UiColoredRectangle() { Color = new Color(255, 0, 0), Transform = new Transform2(new Size2(50, 100)) };

        public void Draw(Transform2 parentTransform)
        {
            cardImage.Draw(parentTransform);
        }
    }
}
