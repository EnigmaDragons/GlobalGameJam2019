using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Physics;
using System;

namespace MonoDragons.Core.UserInterface
{
    public abstract class VisualClickableUIElement : ClickableUIElement, IVisual
    {
        public VisualClickableUIElement(Rectangle area, float scale = 1)
            : base(area, scale) { }

        public VisualClickableUIElement(Rectangle area, Func<bool> isEnabled, float scale = 1) 
            : base(area, isEnabled, scale) {}

        public abstract void Draw(Transform2 parentTransform);
    }
}
