using MonoDragons.GGJ.Gameplay;

namespace MonoDragons.GGJ
{
    public sealed class GameData
    {
        public CharacterState CowboyState { get; set; } = new CharacterState(Player.Cowboy, 50);
        public CharacterState HouseState { get; set; } = new CharacterState(Player.House, 100);
    }
}