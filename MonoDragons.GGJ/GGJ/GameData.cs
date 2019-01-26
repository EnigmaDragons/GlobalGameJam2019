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
        public Dictionary<int, CardState> AllCards { get; set; } = new Dictionary<int, CardState>();
        public CharacterState CowboyState { get; set; }
        public CharacterState HouseState { get; set; }
        
        // Accessors
        public CharacterState this[Player player] => player == Player.Cowboy ? CowboyState : HouseState;
        public Card Card(int cardId) => Cards.Create(AllCards[cardId]);
    }
}