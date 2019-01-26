using MonoDragons.GGJ.Data;
using MonoDragons.GGJ.Gameplay;

namespace MonoDragons.GGJ
{
    public sealed class GameData
    {
        public int CurrentTurn { get; set; }
        public CharacterState CowboyState { get; set; } = new CharacterState(Player.Cowboy, 50, new Deck(Cards.GetCardById(CardName.DeadEye), Cards.GetCardById(CardName.SixShooterThingy), Cards.GetCardById(CardName.YEEHAW)));
        public CharacterState HouseState { get; set; } = new CharacterState(Player.House, 100, new Deck(Cards.GetCardById(CardName.Lazer), Cards.GetCardById(CardName.WaterLeak), Cards.GetCardById(CardName.ElectricShockSuperAttack)));
        public CharacterState this[Player player] => player == Player.Cowboy ? CowboyState : HouseState;
    }
}