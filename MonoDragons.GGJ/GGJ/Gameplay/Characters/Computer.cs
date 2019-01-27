using System;
using MonoDragons.Core;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.GGJ.Gameplay
{
    public class Computer : IHouseChar
    {
        private readonly UiImage _sprite = new UiImage { Image = "Appliances/computer", Transform = new Transform2(UI.OfScreen(0.76f, 0.375f), new Size2(250, 250)) };
        
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