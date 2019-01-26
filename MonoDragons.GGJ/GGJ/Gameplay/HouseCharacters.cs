using System;
using MonoDragons.Core;
using MonoDragons.Core.Engine;

namespace MonoDragons.GGJ.Gameplay
{
    public class HouseCharacters : IVisualAutomaton
    {
        private readonly MustInit<IVisualAutomaton> _char = new MustInit<IVisualAutomaton>("House Current Char");
        
        public HouseCharacters Initialized(IVisualAutomaton current)
        {
            _char.Init(current);
            return this;
        }

        public void Update(TimeSpan delta)
        {
            _char.Get().Update(delta);
        }

        public void Draw(Transform2 parentTransform)
        {
            _char.Get().Draw(parentTransform);
        }
    }
}