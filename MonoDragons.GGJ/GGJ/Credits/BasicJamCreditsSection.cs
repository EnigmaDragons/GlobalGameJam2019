using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Animations;
using MonoDragons.Core.Physics;
using MonoDragons.Core.Text;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Credits
{
    public abstract class BasicJamCreditSegment : IAnimation
    {
        private readonly List<HorizontalFlyInAnimation> _elements = new List<HorizontalFlyInAnimation>();

        private int _countdown;

        public abstract Color RoleColor { get; }
        public abstract string Role { get; }
        public abstract string Name { get; }

        public void Update(TimeSpan delta)
        {
            _elements.ForEach(x => x.Update(delta));
        }

        public void Draw(Transform2 parentTransform)
        {
            _elements.ForEach(x => x.Draw(parentTransform));
        }

        public void Start(Action onFinished)
        {
            var yStart = Rng.Int(0.02.VH(), 0.40.VH());

            _elements.Add(new HorizontalFlyInAnimation(
                new Label
                {
                    Text = Role,
                    Transform = new Transform2(new Vector2(-1000, yStart), new Size2(800, 100)),
                    Font = DefaultFont.Header,
                    TextColor = UiConsts.DarkBrown
                }));
            _elements.Add(new HorizontalFlyInAnimation(
                    new Label
                    {
                        Text = Name,
                        Transform = new Transform2(new Vector2(1720, yStart + 135), new Size2(800, 75)),
                        Font = DefaultFont.Large,
                        TextColor = UiConsts.DarkBrown
                    })
                { FromDir = HorizontalDirection.Right, ToDir = HorizontalDirection.Left });

            _countdown = _elements.Count;
            _elements.ForEach(x => x.Start(() => FinishedOne(onFinished)));
        }

        private void FinishedOne(Action onFinished)
        {
            _countdown--;
            if (_countdown == 0)
                onFinished();
        }
    }
}