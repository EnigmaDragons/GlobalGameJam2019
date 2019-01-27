using System;
using MonoDragons.GGJ.Gameplay;
using System.Collections.Generic;
using MonoDragons.GGJ.Data;

namespace MonoDragons.GGJ
{
    public sealed class GameData
    {
        public int CurrentCardId { get; set; } = 0;
        public int CurrentTurn { get; set; }
        public Phase CurrentPhase { get; set; }
        public int CurrentLevel { get; set; } = 0;
        public Dictionary<int, CardState> AllCards { get; } = new Dictionary<int, CardState>();
        public CharacterState CowboyState { get; set; }
        public CharacterState HouseState { get; set; }

        public void InitLevel(int level, CharacterState cowboy, CharacterState house)
        {
            CurrentLevel = level;
            CurrentTurn = 0;
            CowboyState = cowboy;
            HouseState = house;
        }

        // Accessors
        public CharacterState this[Player player]
        {
            get
            {
                if (CowboyState == null || HouseState == null)
                    throw new InvalidOperationException("Game State Has Not Been Initialized Yet");
                return player == Player.Cowboy ? CowboyState : HouseState;
            }
        }

        public CardView Card(int cardId) => Cards.Create(AllCards[cardId]);
    }
}