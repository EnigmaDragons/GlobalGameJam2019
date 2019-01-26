using MonoDragons.GGJ.Gameplay;

namespace MonoDragons.GGJ
{
    public sealed class GameData
    {
        public CharacterState CowboyState { get; set; } = new CharacterState();
        public CharacterState HouseState { get; set; } = new CharacterState();
    }
}