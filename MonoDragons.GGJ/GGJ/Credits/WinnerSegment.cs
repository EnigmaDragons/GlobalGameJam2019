using System;
using MonoDragons.Core;
using MonoDragons.Core.Animations;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Physics;
using MonoDragons.Core.Text;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Credits
{
    public class WinnerSegment : IAnimation
    {
        private readonly Player _winner;
        private bool _isStarted;
        private VerticalFlyInAnimation _animation;

        public WinnerSegment(Player winner)
        {
            _winner = winner;
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
            _animation = Create();
            _animation.Start(onFinished);
            _isStarted = true;
        }

        private VerticalFlyInAnimation Create()
        {
            return new VerticalFlyInAnimation(new SignView($"{_winner} wins!"))
            {
                FromDir = VerticalDirection.Down,
                ToDir = VerticalDirection.Up,
                Drift = 200,
                DurationIn = TimeSpan.FromMilliseconds(200),
                DurationWait = TimeSpan.FromMilliseconds(3000),
                DurationOut = TimeSpan.FromMilliseconds(200)
            };
        }

        private class SignView : IVisual
        {
            private UiImage _sign;
            private Label _gameOverLabel;

            public SignView(string text)
            {
                _sign = new UiImage
                {
                    Image = "UI/sign",
                    Transform = new Transform2(UI.OfScreen(0.2f, 1.8f), UI.OfScreenSize(0.6f, 0.2f))
                };
                _gameOverLabel = new Label { Transform = new Transform2(UI.OfScreen(0.2f, 1.8f), UI.OfScreenSize(0.6f, 0.2f)), 
                    Font = DefaultFont.Header, TextColor = UiConsts.DarkBrown, Text = text};
            }
            
            public void Draw(Transform2 parentTransform)
            {
                _sign.Draw(parentTransform);
                _gameOverLabel.Draw(parentTransform);
            }
        }
    }
}