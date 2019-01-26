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
    public class CardRevealer : IVisual
    {
        public Optional<Card> Card { get; set; }
        public bool IsRevealed { get; set; }
        private Transform2 location;

        public CardRevealer(Vector2 location, bool isRevealed = false) : this(location, new Optional<Card>(), isRevealed) { }
        public CardRevealer(Vector2 location, Card card, bool isRevealed = false) : this(location, new Optional<Card>(card), isRevealed) { }
        public CardRevealer(Vector2 location, Optional<Card> card, bool isRevealed)
        {
            this.location = new Transform2(location);
            Card = card;
            IsRevealed = isRevealed;
        }

        public void Draw(Transform2 parentTransform)
        {
            if (IsRevealed && Card.HasValue)
                Card.Value.Draw(location);
        }
    }
}
