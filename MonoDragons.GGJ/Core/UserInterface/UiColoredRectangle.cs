using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Graphics;
using MonoDragons.Core.Physics;
using System;

namespace MonoDragons.Core.UserInterface
{
    public sealed class UiColoredRectangle : IVisual
    {
        private Color _color;
        private Texture2D _background;

        public Transform2 Transform { get; set; }
        public Func<bool> IsActive { get; set; } = () => true;

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                UpdateTexture();
            }
        }

        public UiColoredRectangle()
        {
            Transform = new Transform2(new Size2(400, 100));
            _background = new RectangleTexture(Color.Transparent).Create();
        }

        public void Draw(Transform2 parentTransform)
        {
            if (IsActive())
                UI.Draw(_background, (parentTransform + Transform).ToRectangle(), Color.White);
        }

        private void UpdateTexture()
        {
            _background = new RectangleTexture(_color).Create();
        }
    }
}
