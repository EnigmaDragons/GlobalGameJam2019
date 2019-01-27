using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Render;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Gameplay
{
    public class Cowboy : IVisualAutomaton
    {
        private enum CharState
        {
            Idle,
            Walking,
            Running,
        }

        private readonly DamageNumbersView _dmgView = new DamageNumbersView(Player.Cowboy);
        private readonly Vector2 _dmgOffset = new Vector2(68, 0);
        private readonly DictionaryWithDefault<CharState, SpriteAnimation> _anims = 
            new DictionaryWithDefault<CharState, SpriteAnimation>(Anim("__Hoodie_idle with gun"))
            {
                { CharState.Walking, Anim("__Hoodie_walk with gun") },
                { CharState.Running, Anim("__Hoodie_run with gun") }
            };
        
        private const float _scale = 0.5f;
        private float _totalMovementMs = 1.0f;
        private float _elapsedMs = 1.0f;
        private Vector2 _previous;
        private Vector2 _destination;
        private CharState _state = CharState.Idle;
        private Action _onArrival = () => {};
        private bool _isMoving;

        private Vector2 _loc = GetLoc(new Vector2(-400, UI.OfScreenHeight(0.375f)),
            new Vector2(-400, UI.OfScreenHeight(0.375f)), 1.0f);

        public Cowboy()
        {
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
            Event.Subscribe<LevelSetup>(OnLevelSetup, this);
        }

        private void OnLevelSetup(LevelSetup obj)
        {
            _loc = GetLoc(new Vector2(-400, UI.OfScreenHeight(0.375f)), new Vector2(-400, UI.OfScreenHeight(0.375f)), 1.0f);
            Enter();
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            if (e.Winner == Player.Cowboy)
                MoveTo(CharState.Walking, UI.OfScreenWidth(1.0f) + 400, TimeSpan.FromMilliseconds(3200), 
                    () => Event.Publish(new FinishedLevel { LevelNumber = e.LevelNumber, IsGameOver = e.IsGameOver }));
        }

        public void Update(TimeSpan delta)
        {
            _dmgView.Update(delta);
            _anims[_state].Update(delta);
            UpdateMovement(delta);
        }

        private void UpdateMovement(TimeSpan delta)
        {
            if (!_isMoving)
                return;

            if (_elapsedMs < _totalMovementMs)
                _elapsedMs += (float) delta.TotalMilliseconds;
            _loc = GetLoc(_previous, _destination, MathHelper.Clamp(_elapsedMs / _totalMovementMs, 0, 1));
            
            if (_loc.ToPoint() != _destination.ToPoint())
                return;

            _state = CharState.Idle;
            _onArrival();
            _isMoving = false;
        }

        public void Draw(Transform2 parentTransform)
        {
            _dmgView.Draw(parentTransform + _loc + _dmgOffset);
            _anims[_state].Draw(parentTransform + _loc);
        }

        private void Enter()
        {
            MoveTo(CharState.Walking, 60, TimeSpan.FromMilliseconds(2000), () => {});
        }
        
        private Cowboy MoveTo(CharState state, int endX, TimeSpan duration, Action onArrival)
        {
            _onArrival = onArrival;
            _isMoving = true;
            _state = state;
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
