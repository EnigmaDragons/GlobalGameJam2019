using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Animations;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.GGJ.Credits
{
    public class TitleCreditSegment : IAnimation
    {
        private readonly Action _onFinished;
        private bool _isStarted;
        private VerticalFlyInAnimation _animation;

        public TitleCreditSegment(Action onFinished)
        {
            _onFinished = onFinished;
        }
        
        public void Update(TimeSpan delta)
        {
            if (_isStarted)
                _animation.Update(delta);
        }

        public void Draw(Transform2 parentTransform)
        {
            if (_isStarted)
                _animation.Draw(parentTransform);
        }

        public void Start(Action onFinished)
        {
            _animation = CreateTitle();
            _animation.Start(() =>
            {
                _onFinished();
                onFinished();
            });
            _isStarted = true;
        }

        private VerticalFlyInAnimation CreateTitle()
        {
            var titleImage = new UiImage
            {
                Image = "UI/title",
                Transform = new Transform2(new Vector2((1600 - 720) / 2, UI.OfScreenHeight(1.0f)), new Size2(720, 355))
            };
            titleImage.Transform.Location = new Vector2(titleImage.Transform.Location.X, titleImage.Transform.Location.Y + 800);
            return new VerticalFlyInAnimation(titleImage)
            {
                FromDir = VerticalDirection.Down,
                ToDir = VerticalDirection.Up,
                Drift = 200,
                DurationIn = TimeSpan.FromMilliseconds(200),
                DurationWait = TimeSpan.FromMilliseconds(3000),
                DurationOut = TimeSpan.FromMilliseconds(200)
            };
        }
    }
}