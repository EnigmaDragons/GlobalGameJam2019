namespace MonoDragons.GGJ.UiElements.Events
{
    public class AnimationStarted
    {
        public string Name { get; set; }

        public AnimationStarted(string name) => Name = name;
    }
}