using Microsoft.Xna.Framework;
using System;

namespace MonoDragons.Core.UserInterface
{
    public sealed class TintClickableUIElement : VisualClickableUIElement
    {
        private readonly ColoredRectangle _rect;

        public Color Default { get; set; } = Color.Transparent;
        public Color OnHover { get; set; } = Color.FromNonPremultiplied(255, 255, 255, 40);
        public Color OnPress { get; set; } = Color.FromNonPremultiplied(255, 200, 200, 20);
        
        public TintClickableUIElement(Rectangle area, Func<bool> isEnabled, float scale = 1) 
            : base(area, isEnabled, scale)
        {
            _rect = new ColoredRectangle { Transform = new Transform2(area) };
        }

        public override void OnEntered()
        {
            _rect.Color = OnHover;
        }

        public override void OnExitted()
        {
            _rect.Color = Default;
        }

        public override void OnPressed()
        {
            _rect.Color = OnPress;
        }

        public override void OnReleased()
        {
            _rect.Color = OnHover;
        }

        public override void Draw(Transform2 parentTransform)
        {
            _rect.Draw(parentTransform);
        }
    }
}