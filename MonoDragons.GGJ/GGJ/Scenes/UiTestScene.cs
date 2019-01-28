using MonoDragons.Core;
using MonoDragons.Core.Animations;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public class UiTestScene : SimpleScene
    {
        public override void Init()
        {
            Add(new Explosion().Started());
            Add(new CounteredEffect().Started());
        }
    }
}