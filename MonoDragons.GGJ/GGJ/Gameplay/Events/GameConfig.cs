
namespace MonoDragons.GGJ.Gameplay.Events
{
    public struct GameConfig
    {
        public Mode Mode { get; set; }
        public Player HostRole { get; set; }
        public GameData StartingData { get; set; }

        public GameConfig(Mode mode, Player hostRole, GameData startingData) : this()
        {
            Mode = mode;
            HostRole = hostRole;
            StartingData = startingData;
        }
    }
}
