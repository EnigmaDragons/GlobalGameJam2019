using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Animations;
using MonoDragons.Core.Render;

namespace MonoDragons.GGJ.UiElements
{
    public class Explosion : IAnimation
    {
        private const double TotalAnimMs = 880;
        private readonly SpriteAnimation _anim = new SpriteAnimation(
            Enumerable.Range(1, 9)
                .Select(i => new SpriteAnimationFrame($"FX/exp1_00{i}", 1.5f, 0.10f)).ToArray());
        private double _elapsedMs;
        private bool _playing = false;
        private Action _onFinished;

        public void Update(TimeSpan delta)
        {
            if (!_playing)
                return;
            
            _anim.Update(delta);
            if (_elapsedMs < TotalAnimMs)
                _elapsedMs += delta.TotalMilliseconds;
            else
            {
                _playing = false;
                _onFinished();
            }
        }

        public void Draw(Transform2 parentTransform)
        {
            if (!_playing)
                return;
            
            _anim.Draw(parentTransform + new Transform2(new Vector2(-60, -60)));
        }

        public void Start(Action onFinished)
        {
            _onFinished = onFinished;
            _elapsedMs = 0;
            _playing = true;
            _anim.Reset();
        }
    }
}