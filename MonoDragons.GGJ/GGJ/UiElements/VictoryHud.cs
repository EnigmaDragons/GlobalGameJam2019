using System;
using System.Collections.Generic;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Network;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.Text;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Scenes;

namespace MonoDragons.GGJ.UiElements
{
    public class VictoryHud : IVisualAutomatonControl
    {
        private readonly Label _gameOverLabel = new Label { Transform = new Transform2(UI.OfScreen(0.2f, 0.35f), UI.OfScreenSize(0.6f, 0.2f)), 
            Font = DefaultFont.Header, TextColor = UiConsts.DarkBrown };

        private readonly IReadOnlyList<IVisual> _visuals = new List<IVisual>();
        private bool _shouldDisplaySign;

        public ClickUIBranch Branch { get; } = new ClickUIBranch(nameof(VictoryHud), int.MaxValue);

        public VictoryHud()
        {
            _gameOverLabel.IsVisible = () => _shouldDisplaySign;
            var mainMenuButton = new ImageTextButton(new Transform2(UI.OfScreen(0.4f, 0.8f), UI.OfScreenSize(0.20f, 0.10f)), 
                () => Scene.NavigateTo(new MainMenuScene(new NetworkArgs())), "Main Menu", "UI/sign", "UI/sign-hover", "UI/sign-press")
                    { Font = DefaultFont.Large, TextColor = UiConsts.DarkBrown };
            Branch.Add(mainMenuButton);
            _visuals = new List<IVisual>
            {
                new UiImage { Image = "UI/sign", IsActive = () => _shouldDisplaySign, Transform = new Transform2(UI.OfScreen(0.2f, 0.35f), UI.OfScreenSize(0.6f, 0.2f)) },
                _gameOverLabel,
                mainMenuButton,
            };
        }

        public VictoryHud WithText(string text)
        {
            _gameOverLabel.Text = text;
            _shouldDisplaySign = true;
            return this;
        }
        
        public void Draw(Transform2 parentTransform)
        {
            _visuals.ForEach(x => x.Draw(parentTransform));            
        }

        public void Update(TimeSpan delta)
        {
        }
    }
}