using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.GGJ.Gameplay
{
    public class CardView : IVisual
    {
        public const int WIDTH = 160;
        public const int HEIGHT = (int)(7f * (WIDTH / 5f));
        private readonly string _name;
        public CardState State { get; }
        public int Id => State.Id;

        public CardView(CardState state, string imageName)
        {
            State = state;
            _name = imageName;
        }

        public void Draw(Transform2 parentTransform)
        {
            UI.Draw("Cards/" + _name, parentTransform + new Transform2(new Rectangle(0, 0, WIDTH, HEIGHT)));
        }
    }
}
