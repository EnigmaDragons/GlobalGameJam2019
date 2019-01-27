namespace MonoDragons.GGJ.Gameplay.Events
{
    public class DamageTakenMultiplied
    {
        public Player Target { get; set; }
        public decimal Multiplier { get; set; }
    }
}
