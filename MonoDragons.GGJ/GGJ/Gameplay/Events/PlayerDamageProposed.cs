namespace MonoDragons.GGJ.Gameplay.Events
{
    public class PlayerDamageProposed
    {
        public Player Target { get; set; }
        public int Amount { get; set; }
    }
}
