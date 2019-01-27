using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Animations;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Network;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.Text;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.Scenes;
using System;
using System.Collections.Generic;

namespace MonoDragons.GGJ.UiElements
{
    class BattleTopHud : IVisualAutomatonControl
    {
        private static readonly Size2 HpSize = new Size2(68, 68);
        private readonly Label _cowboyHp = new Label { Transform = new Transform2(UI.OfScreen(0.008f, 0.78f), HpSize) };
        private readonly Label _houseHp = new Label { Transform = new Transform2(UI.OfScreen(0.946f, 0.78f), HpSize) };
        private readonly Label _cowboyNrg = new Label { Transform = new Transform2(UI.OfScreen(0.008f, 0.88f), HpSize), TextColor = UiConsts.DarkBrown };
        private readonly Label _houseNrg = new Label { Transform = new Transform2(UI.OfScreen(0.946f, 0.88f), HpSize), TextColor = UiConsts.DarkBrown };
        private readonly Label _gameOverLabel = new Label { Transform = new Transform2(UI.OfScreen(0.2f, 0.35f), UI.OfScreenSize(0.6f, 0.2f)), 
            Font = DefaultFont.Header, TextColor = UiConsts.DarkBrown };

        private readonly ScreenFade _fadeToBlack = new ScreenFade { FromAlpha = 0, ToAlpha = 255, Duration = TimeSpan.FromSeconds(3) };
        private readonly ScreenFade _fadeToGrey = new ScreenFade { FromAlpha = 0, ToAlpha = 160, Duration = TimeSpan.FromMilliseconds(1500) };
        private readonly IReadOnlyList<IVisual> _visuals = new List<IVisual>();
        private readonly MustInit<GameData> _gameData = new MustInit<GameData>("Game Data");
        private readonly Player _player;
        private bool _shouldDisplaySign;
        private bool _isGameOver;
        private bool _isDisconnected;

        public ClickUIBranch Branch { get; } = new ClickUIBranch(nameof(BattleTopHud), int.MaxValue);

        public BattleTopHud(Player p, GameData g)
        {
            _gameOverLabel.IsVisible = () => _shouldDisplaySign;
            _player = p;
            _gameData.Init(g);
            var quitButton = new ImageTextButton(new Transform2(UI.OfScreen(0.4f, 0.8f), UI.OfScreenSize(0.20f, 0.10f)), 
                () => Scene.NavigateTo(new LobbyScene(new NetworkArgs())), "Quit", "UI/sign", "UI/sign-hover", "UI/sign-press", 
                () => _isGameOver || _isDisconnected)
                    { Font = DefaultFont.Large, TextColor = UiConsts.DarkBrown };
            Branch.Add(quitButton);
            _visuals = new List<IVisual>
            {
                new UiImage { Image= "UI/heart", Transform = new Transform2(UI.OfScreen(0.008f, 0.78f), HpSize)},
                new UiImage { Image= "UI/heart", Transform = new Transform2(UI.OfScreen(0.946f, 0.78f), HpSize)},
                _cowboyHp,
                _houseHp,
                new UiImage { Image= "UI/energy", Transform = new Transform2(UI.OfScreen(0.008f, 0.88f), HpSize)},
                new UiImage { Image= "UI/energy", Transform = new Transform2(UI.OfScreen(0.946f, 0.88f), HpSize)},
                _cowboyNrg,
                _houseNrg,
                new UiColoredRectangle { Color = Color.FromNonPremultiplied(0, 0, 0, 160), IsActive = () => _isDisconnected, Transform = new Transform2(new Size2(1920, 1680))},
                new UiColoredRectangle { Color = Color.Black, IsActive = () => _isGameOver, Transform = new Transform2(new Size2(1920, 1680))},
                _fadeToBlack,
                _fadeToGrey,
                new UiImage { Image= "UI/sign", IsActive = () => _shouldDisplaySign, Transform = new Transform2(UI.OfScreen(0.2f, 0.35f), UI.OfScreenSize(0.6f, 0.2f))},
                _gameOverLabel,
                quitButton,
            };
            Event.Subscribe<LevelSetup>(OnLevelSetup, this);
            Event.Subscribe<FinishedLevel>(OnLevelFinished, this);
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
            Event.Subscribe<GameDisconnected>(OnDisconnected, this);
        }

        private void OnLevelSetup(LevelSetup e)
        {
            _cowboyHp.Text = _gameData.Get().CowboyState.HP.ToString();
            _houseHp.Text = _gameData.Get().HouseState.HP.ToString();
        }

        private void OnLevelFinished(FinishedLevel e)
        {
            _shouldDisplaySign = false;
        }

        private void OnDisconnected(GameDisconnected e)
        {
            if (_shouldDisplaySign)
                return;

            _shouldDisplaySign = true;
            _gameOverLabel.Text = "Opponent Disconnected.";
            _fadeToGrey.Start(() => _isDisconnected = true);
        }

        public void Draw(Transform2 parentTransform)
        {
            _visuals.ForEach(x => x.Draw(parentTransform));            
        }

        public void Update(TimeSpan delta)
        {
            _cowboyHp.Text = _gameData.Get().CowboyState.HP.ToString();
            _houseHp.Text = _gameData.Get().HouseState.HP.ToString();
            _cowboyNrg.Text = _gameData.Get().CowboyState.Energy.ToString();
            _houseNrg.Text = _gameData.Get().HouseState.Energy.ToString();
            _fadeToBlack.Update(delta);
            _fadeToGrey.Update(delta);
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            _gameOverLabel.Text = e.Winner == _player ? "You Win!" : "You Defeated!";
            _shouldDisplaySign = true;
            if (e.IsGameOver)
                _fadeToBlack.Start(() => _isGameOver = e.IsGameOver);
        }
    }
}
