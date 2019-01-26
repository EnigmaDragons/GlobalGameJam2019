using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Animations;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.Text;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay;
using System;
using System.Collections.Generic;

namespace MonoDragons.GGJ.UiElements
{
    class BattleTopHud : IVisualAutomaton
    {
        private readonly Label _cowboyHp = new Label { Transform = new Transform2(UI.OfScreen(0.038f, 0.02f), new Size2(92, 92)) };
        private readonly Label _houseHp = new Label { Transform = new Transform2(UI.OfScreen(0.9f, 0.02f), new Size2(92, 92)) };
        private readonly Label _gameOverLabel = new Label { Transform = new Transform2(UI.OfScreen(0.2f, 0.35f), UI.OfScreenSize(0.6f, 0.2f)), Font = DefaultFont.Header, TextColor = UiConsts.DarkBrown };

        private readonly ScreenFade _fade = new ScreenFade { FromAlpha = 0, ToAlpha = 255, Duration = TimeSpan.FromSeconds(3) };
        private readonly IReadOnlyList<IVisual> _visuals = new List<IVisual>();
        private readonly MustInit<GameData> _gameData = new MustInit<GameData>("Game Data");
        private readonly Player _player;
        private bool _shouldDisplaySign;
        private bool _isGameOver;

        public ClickUIBranch Branch { get; } = new ClickUIBranch(nameof(BattleTopHud), int.MaxValue);

        public BattleTopHud(Player p, GameData g)
        {
            _player = p;
            _gameData.Init(g);
            _cowboyHp.Text = _gameData.Get().CowboyState.HP.ToString();
            _houseHp.Text = _gameData.Get().HouseState.HP.ToString();
            var quitButton = new ImageTextButton(new Transform2(UI.OfScreen(0.4f, 0.8f), UI.OfScreenSize(0.20f, 0.10f)), () => Scene.NavigateTo("Lobby"), "Quit", "UI/sign", "UI/sign-hover", "UI/sign-press", () => _isGameOver) { Font = DefaultFont.Large, TextColor = UiConsts.DarkBrown };
            Branch.Add(quitButton);
            _visuals = new List<IVisual>
            {
                new UiImage { Image= "UI/wood-box", Transform = new Transform2(UI.OfScreen(-0.04f, -0.14f), new Size2(300, 300))},
                new UiImage { Image= "UI/wood-box", Transform = new Transform2(UI.OfScreen(0.85f, -0.14f), new Size2(300, 300))},
                new UiImage { Image= "UI/heart", Transform = new Transform2(UI.OfScreen(0.038f, 0.02f), new Size2(92, 92))},
                new UiImage { Image= "UI/heart", Transform = new Transform2(UI.OfScreen(0.9f, 0.02f), new Size2(92, 92))},
                _cowboyHp,
                _houseHp,
                new UiColoredRectangle { Color = Color.Black, IsActive = () => _isGameOver, Transform = new Transform2(new Size2(1920, 1680))},
                _fade,
                new UiImage { Image= "UI/sign", IsActive = () => _shouldDisplaySign, Transform = new Transform2(UI.OfScreen(0.2f, 0.35f), UI.OfScreenSize(0.6f, 0.2f))},
                _gameOverLabel,
                quitButton,
            };
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
        }

        public void Draw(Transform2 parentTransform)
        {
            _visuals.ForEach(x => x.Draw(parentTransform));            
        }

        public void Update(TimeSpan delta)
        {
            _cowboyHp.Text = _gameData.Get().CowboyState.HP.ToString();
            _houseHp.Text = _gameData.Get().HouseState.HP.ToString();
            _fade.Update(delta);
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            _gameOverLabel.Text = e.Winner == _player ? "You Win!" : "You Defeated!";
            _shouldDisplaySign = true;
            _fade.Start(() => _isGameOver = e.IsGameOver);
        }
    }
}
