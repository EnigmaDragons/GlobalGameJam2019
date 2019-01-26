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
        private Vector2 _loc = GetLoc();

        private enum CharState
        {
            Idle
        }

        private CharState _state = CharState.Idle;
        private DictionaryWithDefault<CharState, SpriteAnimation> _anims = 
            new DictionaryWithDefault<CharState, SpriteAnimation>(Anim("__Hoodie_idle")) { };

        public int HP { get; } = 50;

        public void Update(TimeSpan delta)
        {
            _loc = GetLoc();
            _anims[_state].Update(delta);
        }

        public void Draw(Transform2 parentTransform)
        {
            _anims[_state].Draw(parentTransform + _loc);
        }

        private static SpriteAnimation Anim(string baseName)
        {
            const float duration = 0.16f;
            return new SpriteAnimation(
                Enumerable.Range(0, 10)
                    .Select(i => new SpriteAnimationFrame($"Cowboy/{baseName}_00{i}", _scale, duration)).ToArray());
        }

        private static Vector2 GetLoc()
        {
            return new Vector2(60, UI.OfScreenHeight(0.375f));
        }
    }
}
