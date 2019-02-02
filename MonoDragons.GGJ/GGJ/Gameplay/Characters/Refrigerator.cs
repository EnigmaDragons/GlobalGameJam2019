using System;
using MonoDragons.Core;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.GGJ.Gameplay
{
    public class Refrigerator : IHouseChar
    {
        private readonly Sprite _sprite = new Sprite { Image = "Appliances/refrigerator", Transform = new Transform2(UI.OfScreen(0.76f, 0.28f), new Size2(220, 343)) };
        
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