using MonoDragons.Core.Animations;
using MonoDragons.Core.Scenes;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public class UiTestScene : SimpleScene
    {
        public override void Init()
        {
            Add(new Explosion().Started());
        }
    }
}