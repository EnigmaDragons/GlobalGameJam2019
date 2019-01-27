using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Render;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.UiElements;
using MonoDragons.GGJ.UiElements.Events;

namespace MonoDragons.GGJ.Gameplay
{
    public class Cowboy : IVisualAutomaton
    {
        private enum CharState
        {
            Idle,
            Walking,
            Running,
            Celebrating,
        }

        private readonly DamageNumbersView _dmgView = new DamageNumbersView(Player.Cowboy);
        private readonly Vector2 _dmgOffset = new Vector2(68, 0);
        private readonly Vector2 _statusOffset = new Vector2(-45, 210);
        private readonly CharStatuses _statusView = new CharStatuses(Player.Cowboy);
        private readonly DictionaryWithDefault<CharState, SpriteAnimation> _anims;
        
        private float _totalMovementMs = 1.0f;
        private float _elapsedMs = 1.0f;
        private Vector2 _previous;
        private Vector2 _destination;
        private CharState _state = CharState.Idle;
        private Action _onArrival = () => {};
        private bool _isMoving;

        private Vector2 _loc; 

        public Cowboy(Phase phase, float scale = 0.5f)
        {
            if (phase != Phase.Setup)
                _loc = GetLoc(new Vector2(60, UI.OfScreenHeight(0.375f)), new Vector2(60, UI.OfScreenHeight(0.375f)), 1.0f);
            else
                _loc = GetLoc(new Vector2(-400, UI.OfScreenHeight(0.375f)), new Vector2(-400, UI.OfScreenHeight(0.375f)), 1.0f);
            _anims = new DictionaryWithDefault<CharState, SpriteAnimation>(Anim("__Hoodie_idle with gun", scale))
            {
                { CharState.Walking, Anim("__Hoodie_walk with gun", scale) },
                { CharState.Running, Anim("__Hoodie_run with gun", scale) },
                { CharState.Celebrating, Anim("__Hoodie_celebrate", scale, 1.0f) }
            };
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
                Celebrate(e);
        }

        private void Celebrate(PlayerDefeated e)
        {
            MoveTo(CharState.Idle, (int) _loc.X, TimeSpan.FromMilliseconds(800), () =>
                MoveTo(CharState.Celebrating, (int) _loc.X, TimeSpan.FromMilliseconds(1600), () =>
                    MoveTo(CharState.Idle, (int) _loc.X, TimeSpan.FromMilliseconds(600), () =>
                    {
                        Event.Publish(new FinishedCelebrating());
                        MoveTo(CharState.Running, UI.OfScreenWidth(1.0f) + 400, TimeSpan.FromMilliseconds(3200),
                            () => Event.Publish(new FinishedLevel { LevelNumber = e.LevelNumber, IsGameOver = e.IsGameOver }));
                    })));
        }

        public void Update(TimeSpan delta)
        {
            _anims[_state].Update(delta);            
            _dmgView.Update(delta);
            _statusView.Update(delta);
            UpdateMovement(delta);
        }

        private void UpdateMovement(TimeSpan delta)
        {
            if (!_isMoving)
                return;

            if (_elapsedMs < _totalMovementMs)
                _elapsedMs += (float) delta.TotalMilliseconds;
            _loc = GetLoc(_previous, _destination, MathHelper.Clamp(_elapsedMs / _totalMovementMs, 0, 1));
            
            if (_elapsedMs < _totalMovementMs)
                return;

            _state = CharState.Idle;
            _isMoving = false;
            _onArrival();
        }

        public void Draw(Transform2 parentTransform)
        {
            _anims[_state].Draw(parentTransform + _loc);
            _dmgView.Draw(parentTransform + _loc + _dmgOffset);
            _statusView.Draw(parentTransform + _loc + _statusOffset);
        }

        public Cowboy Enter()
        {
            MoveTo(CharState.Walking, 60, TimeSpan.FromMilliseconds(1780), () => {});
            return this;
        }
        
        private Cowboy MoveTo(CharState state, int endX, TimeSpan duration, Action onArrival)
        {
            _onArrival = onArrival;
            _state = state;
            _previous = _loc;
            _destination = new Vector2(endX, UI.OfScreenHeight(0.375f));
            _elapsedMs = 0;
            _totalMovementMs = (float)duration.TotalMilliseconds;
            _isMoving = true;
            return this;
        }
        
        private static SpriteAnimation Anim(string baseName, float scale, float finalFrameDurationInSeconds = 0.13f)
        {
            const float duration = 0.13f;
            return new SpriteAnimation(
                Enumerable.Range(0, 10)
                    .Select(i => new SpriteAnimationFrame($"Cowboy/{baseName}_00{i}", scale, 
                        i == 9 ? finalFrameDurationInSeconds : duration)).ToArray());
        }

        private static Vector2 GetLoc(Vector2 start, Vector2 destination, float elapsed)
        {
            return Vector2.Lerp(start, destination, elapsed);
        }
    }
}
