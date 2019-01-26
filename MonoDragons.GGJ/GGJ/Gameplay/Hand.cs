using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.GGJ.Gameplay
{
    public class Hand : IVisualControl
    {
        public ClickUIBranch Branch { get; private set; }
        public List<Card> Cards { get; private set; }
        private bool isHouse;
        
        public Hand(bool isHouse, List<Card> cards)
        {
            this.isHouse = isHouse;
            Cards = cards;
            Branch = new ClickUIBranch("Hand", 1);
            for (var i = 0; i < cards.Count; i++)
            {
                var ii = i;
                Branch.Add(new SimpleClickable(new Rectangle(100 + i * 100, 700, Card.WIDTH, Card.HEIGHT), () => CardSelected(ii)));
            }
        }

        private void CardSelected(int cardIndex)
        {
            Event.Publish(new CardSelected(Cards[cardIndex], isHouse));
        }

        public void Draw(Transform2 parentTransform)
        {
            for (var i = 0; i < Cards.Count; i++)
                Cards[i].Draw(new Transform2(new Vector2(100 + i * 100, 700)));
        }
    }
}
