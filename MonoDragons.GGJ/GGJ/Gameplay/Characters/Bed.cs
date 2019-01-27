using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.UserInterface;
using System;

namespace MonoDragons.GGJ.Gameplay
{
    class Bed : IHouseChar
    {
        private readonly Sprite _sprite = new Sprite { Image = "Appliances/bed", Transform = new Transform2(UI.OfScreen(0.76f, 0.375f), new Size2(300, 238)) };
        
        public void Draw(Transform2 parentTransform)
        {
            _sprite.Draw(parentTransform);
        }

        public void Update(TimeSpan delta)
        {
            
        }

        public Transform2 Transform => _sprite.Transform;
    }
}
