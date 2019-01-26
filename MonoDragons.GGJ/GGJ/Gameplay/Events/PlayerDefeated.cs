
namespace MonoDragons.GGJ.Gameplay.Events
{
    public class PlayerDefeated
    {
        public Player Winner { get; set; }
        public bool IsGameOver { get; set; }
    }
}
