using MonoDragons.Core.EventSystem;

namespace MonoDragons.GGJ.Gameplay
{
    public sealed class LevelProgression
    {
        private readonly GameData _data;

        public LevelProgression(GameData data)
        {
            _data = data;
            Event.Subscribe<FinishedLevel>(OnLevelFinished, this);
        }

        private void OnLevelFinished(FinishedLevel e)
        {
            if (!e.IsGameOver)
            {
                Event.Publish(new NextLevelRequested { Level = _data.CurrentLevel + 1 });
            }
        }
    }
}