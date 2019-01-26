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
    public class Hand : IVisual
    {
        public ClickUIBranch ClickUiBranch { get; private set; }
        public List<Card> Cards { get; private set; }
        private List<bool> IsCardsSelected;

        public Hand(params Card[] cards)
        {
            Cards = new List<Card>();
            IsCardsSelected = new List<bool>();
            ClickUiBranch = new ClickUIBranch("Hand", 1);
            for (var i = 0; i < cards.Length; i++)
            {
                var ii = i;
                Cards.Add(cards[i]);
                IsCardsSelected.Add(false);
                ClickUiBranch.Add(new SimpleClickable(new Rectangle(100 + i * 100, 700, 50, 100), () => CardSelected(ii)));
            }
        }

        private void CardSelected(int cardIndex)
        {
            IsCardsSelected[cardIndex] = true;
            Event.Publish(new CardSelected(Cards[cardIndex]));
        }

        public void Draw(Transform2 parentTransform)
        {
            for (var i = 0; i < Cards.Count; i++)
                if (IsCardsSelected[i])
                    Cards[i].Draw(new Transform2(new Vector2(100 + i * 100, 650)));
                else
                    Cards[i].Draw(new Transform2(new Vector2(100 + i * 100, 700)));
        }
    }
}
