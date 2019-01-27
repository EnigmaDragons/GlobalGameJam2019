namespace MonoDragons.GGJ.Gameplay.Events
{
    public class CounterEffectQueued
    {
        public Player Caster { get; set; }
        public CardType Type { get; set; }
        public object Event { get; set; }
    }
}
