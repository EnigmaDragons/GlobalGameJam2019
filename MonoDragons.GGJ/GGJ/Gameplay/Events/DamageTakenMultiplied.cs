namespace MonoDragons.GGJ.Gameplay.Events
{
    public enum MultiplierType : int
    {
        Zero = 0,
        Half = 1,
        Double = 4,
    }
    
    public class DamageTakenMultiplied
    {
        public Player Target { get; set; }
        public MultiplierType Type { get; set; }
    }
}
