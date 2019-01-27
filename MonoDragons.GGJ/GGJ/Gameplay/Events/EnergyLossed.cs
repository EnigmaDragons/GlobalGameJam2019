namespace MonoDragons.GGJ.Gameplay.Events
{
    public class EnergyLossed
    {
        public Player Target { get; set; }
        public int Amount { get; set; }
    }
}
