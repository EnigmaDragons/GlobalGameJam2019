﻿using MonoDragons.GGJ.Data;

namespace MonoDragons.GGJ.Gameplay
{
    public class CardState
    {
        public int Id { get; set; }
        public CardName CardName { get; set; }
        public CardType Type { get; set; }
        public int PredictedDamage { get; set; }
        public int PredictedBlock { get; set; }
    }
}
