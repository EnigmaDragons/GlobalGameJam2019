using MonoDragons.GGJ.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.GGJ.Data
{
    public class Cards
    {
        private static Dictionary<int, Card> _cards = new Dictionary<int, Card>() {
            { 0, new Card("", 0) },
            { 1, new Card("CowboyCard0", 1) },
            { 2, new Card("CowboyCard1", 2) },
            { 3, new Card("CowboyCard2", 3) },
            { 4, new Card("SmartHouseCards0", 4) },
            { 5, new Card("SmartHouseCards1", 5) },
            { 6, new Card("SmartHouseCards2", 6) }
        };

        public static Card GetCardById(int id)
        {
            return _cards[id];
        }
    }
}
