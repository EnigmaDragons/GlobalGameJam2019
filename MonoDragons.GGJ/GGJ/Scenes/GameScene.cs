using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay;
using System.Collections.Generic;

namespace MonoDragons.GGJ.Scenes
{
    public class GameScene : ClickUiScene
    {

        public override void Init()
        {
            Add(new Label { Text = "You are in game!", Transform = new Transform2(new Vector2(0, 0), new Size2(1600, 800)) });
            Add(new Hand(new Card(), new Card(), new Card()));
        }
    }
}
