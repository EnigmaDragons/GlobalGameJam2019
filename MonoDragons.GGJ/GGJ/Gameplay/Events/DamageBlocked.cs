namespace MonoDragons.GGJ.Gameplay.Events
{
    public class DamageBlocked
    {
        public Player Target { get; set; }
        public int Amount { get; set; }
    }
}
