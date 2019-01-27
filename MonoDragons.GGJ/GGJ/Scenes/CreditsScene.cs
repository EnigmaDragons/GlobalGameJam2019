using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.AudioSystem;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public class CreditsScene : ClickUiScene
    {
        private readonly Player _winner;

        public CreditsScene(Player winner)
        {
            _winner = winner;
        }
        
        public override void Init()
        {
            Sound.Music("credits").Play();
            Add(new Sprite { Image = "Outside/desert_bg", Transform = new Transform2(UI.OfScreenSize(1.0f, 1.0f))});
            Add(new Sprite { Image = "Outside/desert_front", Transform = new Transform2(UI.OfScreenSize(1.0f, 1.0f))});
            Add(new VictoryHud().WithText($"{_winner} Wins!"));
        }
    }
}