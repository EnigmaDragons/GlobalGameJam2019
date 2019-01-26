using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Render;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.GGJ.Gameplay
{
    public class Cowboy : IVisualAutomaton
    {
        private const float _scale = 0.5f;
        private float _totalMovementMs = 1.0f;
        private float _elapsedMs = 1.0f;
        private Vector2 _previous;
        private Vector2 _destination;
        private CharState _state = CharState.Idle;

        private Vector2 _loc = GetLoc(new Vector2(-400, UI.OfScreenHeight(0.375f)),
            new Vector2(-400, UI.OfScreenHeight(0.375f)), 1.0f);

        private enum CharState
        {
            Idle,
            Walking
        }

        private readonly DictionaryWithDefault<CharState, SpriteAnimation> _anims = 
            new DictionaryWithDefault<CharState, SpriteAnimation>(Anim("__Hoodie_idle"))
            {
                { CharState.Walking, Anim("__Hoodie_walk") }
            };

        public void Update(TimeSpan delta)
        {
            if (_elapsedMs < _totalMovementMs)
                _elapsedMs += (float)delta.TotalMilliseconds;
            _loc = GetLoc(_previous, _destination, MathHelper.Clamp(_elapsedMs / _totalMovementMs, 0, 1));
            Logger.Write(_loc);
            Logger.Write(_elapsedMs);
            if (_loc == _destination)
                _state = CharState.Idle;
            _anims[_state].Update(delta);
        }

        public void Draw(Transform2 parentTransform)
        {
            _anims[_state].Draw(parentTransform + _loc);
        }

        public Cowboy Enter()
        {
            return Walk(60, TimeSpan.FromMilliseconds(2000));
        }
        
        private Cowboy Walk(int endX, TimeSpan duration)
        {
            _state = CharState.Walking;
            _previous = _loc;
            _destination = new Vector2(endX, UI.OfScreenHeight(0.375f));
            _elapsedMs = 0;
            _totalMovementMs = (float)duration.TotalMilliseconds;
            return this;
        }
        
        private static SpriteAnimation Anim(string baseName)
        {
            const float duration = 0.16f;
            return new SpriteAnimation(
                Enumerable.Range(0, 10)
                    .Select(i => new SpriteAnimationFrame($"Cowboy/{baseName}_00{i}", _scale, duration)).ToArray());
        }

        private static Vector2 GetLoc(Vector2 start, Vector2 destination, float elapsed)
        {
            return Vector2.Lerp(start, destination, elapsed);
        }
    }
}
