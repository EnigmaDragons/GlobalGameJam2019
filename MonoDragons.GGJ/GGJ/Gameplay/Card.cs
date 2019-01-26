using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.GGJ.Gameplay
{
    public class Card : IVisual
    {
        //5 : 7 ratio
        public const int WIDTH = 250;
        public const int HEIGHT = 350;
        private readonly string _name;
        private readonly Action _action;
        public readonly int Id;

        public Card(string name, int id, Action action)
        {
            _name = name;
            Id = id;
            Event.Subscribe<AllCardsSelected>(OnCardSelected, this);
            _action = action;
        }

        public void Draw(Transform2 parentTransform)
        {
            UI.Draw("Cards/" + _name, parentTransform + new Transform2(new Rectangle(0, 0, WIDTH, HEIGHT)));
        }

        private void OnCardSelected(AllCardsSelected e)
        {
            if (e.CowboyCard == Id || e.HouseCard == Id)
                _action();
        }
    }
}
