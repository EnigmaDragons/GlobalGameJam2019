using System;
using System.Linq;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Render;

namespace MonoDragons.GGJ.Gameplay
{
    public class Cowboy : IVisualAutomaton
    {
        private enum CharState
        {
            Idle
        }

        private CharState _state = CharState.Idle;
        private DictionaryWithDefault<CharState, SpriteAnimation> _anims = 
            new DictionaryWithDefault<CharState, SpriteAnimation>(Anim("__Hoodie_idle")) { };

        public void Update(TimeSpan delta)
        {
            _anims[_state].Update(delta);
        }

        public void Draw(Transform2 parentTransform)
        {
            _anims[_state].Draw(parentTransform);
        }

        private static SpriteAnimation Anim(string baseName)
        {
            const float duration = 0.16f;
            return new SpriteAnimation(
                Enumerable.Range(0, 10)
                    .Select(i => new SpriteAnimationFrame($"Cowboy/{baseName}_00{i}", duration)).ToArray());
        }
    }
}
