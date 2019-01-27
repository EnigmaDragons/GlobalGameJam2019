using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.UserInterface;
using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Gameplay;

namespace MonoDragons.GGJ.UiElements
{
    class LevelBackground : IVisualAutomaton
    {
        private readonly Sprite _bg;
        private readonly Sprite _next;
        private float _totalMovementMs = 12000f;
        private float _elapsedMs = 0f;
        private float _destination;
        private float _currentX;
        private bool _isMoving;
        private int _newLevel;

        public LevelBackground()
        {
            _bg = new Sprite { Transform = new Transform2(GetBgSize()) };   
            _next = new Sprite { Transform = new Transform2(GetBgSize()), IsActive = () => _isMoving};
            Event.Subscribe<NextLevelRequested>(OnLevelRequested, this);
        }

        private void OnLevelRequested(NextLevelRequested e)
        {
            TransitionTo(e.Level);
            _newLevel = e.Level;
        }
        
        private void TransitionTo(int level)
        {
            var image = $"House/level{level}";
            _currentX = 0;
            _elapsedMs = 0;
            _next.Image = image;
            _isMoving = true;
        }
        
        public void Draw(Transform2 parentTransform)
        {
            _bg.Draw(parentTransform + new Vector2(-_currentX, 0));
            _next.Draw(parentTransform + new Vector2(-_currentX + UI.OfScreenWidth(1.0f), 0));
        }

        public void Update(TimeSpan delta)
        {
            _bg.Transform.Size = GetBgSize();
            _destination = UI.OfScreenWidth(1.0f);
            if (_isMoving)
            {
                _elapsedMs += (float)delta.TotalMilliseconds;
                _currentX = MathHelper.Lerp(_currentX, _destination, _elapsedMs / _totalMovementMs);
            }
            if (_isMoving && Math.Abs(_currentX - _destination) < 0.001f)
            {
                _isMoving = false;
                _bg.Image = _next.Image;
                _next.Image = "None";
                _currentX = 0;
                Event.Publish(new LevelSetup { CurrentLevel = _newLevel });
            }
        }

        private Size2 GetBgSize()
        {
            var screenWidth = UI.OfScreenWidth(1.0f);
            var size = new Size2(screenWidth, (int)(screenWidth * (1 / UiConsts.BgRatio)));
            return size;
        }
    }
}
