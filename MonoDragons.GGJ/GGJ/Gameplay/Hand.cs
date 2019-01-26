using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.GGJ.Gameplay
{
    public class Hand : IVisual
    {
        public List<Card> cards;

        public Hand(params Card[] cards)
        {
            this.cards = cards.ToList();
        }

        public void Draw(Transform2 parentTransform)
        {
            for (var i = 0; i < cards.Count; i++)
                cards[i].Draw(new Transform2(new Vector2(100 + i * 100, 700)));
        }
    }
}
