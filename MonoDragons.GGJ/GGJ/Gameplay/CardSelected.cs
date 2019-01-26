using MonoDragons.Core.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.GGJ.Gameplay
{
    public struct CardSelected
    {
        public Card Card;

        public CardSelected(Card card)
        {
            Card = card;
        }
    }
}
