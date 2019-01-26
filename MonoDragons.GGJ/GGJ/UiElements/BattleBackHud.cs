using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.UserInterface;
using System.Collections.Generic;

namespace MonoDragons.GGJ.UiElements
{
    class BattleBackHud : IVisual
    {
        private List<IVisual> _visuals = new List<IVisual>
        {
            new UiImage{ Image= "House/floor", Transform = new Transform2(UI.OfScreenSize(1.0f, 1.0f))} 
        };

        public void Draw(Transform2 parentTransform)
        {
            _visuals.ForEach(x => x.Draw());
        }
    }
}
