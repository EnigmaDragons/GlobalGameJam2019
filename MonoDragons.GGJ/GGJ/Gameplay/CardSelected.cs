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
        public int CardId;
        public Player Player;

        public CardSelected(int cardId, Player player)
        {
            CardId = cardId;
            Player = player;
        }
    }
}
