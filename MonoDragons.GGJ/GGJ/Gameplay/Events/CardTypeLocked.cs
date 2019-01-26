namespace MonoDragons.GGJ.Gameplay.Events
{
    public class CardTypeLocked
    {
        public Player Target { get; set; }
        public CardType Type { get; set; }
    }
}
