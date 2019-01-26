using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.GGJ.Gameplay
{
    public class Card : IVisual
    {
        //5 : 7 ratio
        public const int WIDTH = 250;
        public const int HEIGHT = 350;
        private readonly string _name;
        public CardState State { get; }
        public int Id => State.Id;

        public Card(CardState state, string imageName)
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
