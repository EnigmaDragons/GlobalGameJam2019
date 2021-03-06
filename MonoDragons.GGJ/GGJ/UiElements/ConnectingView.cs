﻿using System;
using System.Diagnostics;
using System.Reflection;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.Network;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay.Events;
using MonoDragons.GGJ.Scenes;

namespace MonoDragons.GGJ.UiElements
{
    public class ConnectingView : SceneContainer, IVisualAutomatonControl, IInitializable
    {
        private readonly UserSettings _settings;
        private readonly Action _onHide;
        private Label _addressLabel;
        private NetworkArgs _netArgs;
        private Optional<GameConfig> _config;
        private bool _isConnecting;
        private bool _isConnected;
        
        public ClickUIBranch Branch { get; } = new ClickUIBranch(nameof(ConnectingView), int.MaxValue);

        public ConnectingView(UserSettings settings, Action onHide)
        {
            _settings = settings;
            _onHide = onHide;
        }
        
        public void Init()
        {
            Add(new UiImage{ Image = "UI/wood-textbox", Transform = new Transform2(UI.OfScreen(0.34f, 0.58f), UI.OfScreenSize(0.32f, 0.16f)), IsActive = () => _isConnecting});
            _addressLabel = new Label
            {
                Transform = new Transform2(UI.OfScreen(0.34f, 0.58f), UI.OfScreenSize(0.32f, 0.16f)),
                IsVisible = () => _isConnecting
            };
            Add(_addressLabel);
            var cancelButton = Buttons.Wood("Cancel", UI.OfScreenSize(0.41f, 0.79f).ToPoint(), () =>
            {
                Multiplayer.Disconnect();
                _isConnecting = false;
                _onHide();
            }, () => _isConnecting);
            Add(new Label
            {
                Text = "Connected to opponent. Starting game...",
                Transform = new Transform2(UI.OfScreen(0.34f, 0.58f), UI.OfScreenSize(0.32f, 0.16f)),
                IsVisible = () => _isConnected
            });
            Add(new ActionAutomaton(() => { if (_isConnected && _config.HasValue) { Scene.NavigateTo(new GameScene(_config.Value, _netArgs.ShouldHost)); }}));
            Add(new ActionAutomaton(() => { if (_isConnected && (_netArgs?.ShouldHost ?? false)) { Event.Publish(_config.Value); }}));
            
            Add(cancelButton);
            Branch.Add(cancelButton);

            Input.On(Control.Menu, LaunchConnectingClient);
        }

        public void Connect(NetworkArgs netArgs, Optional<GameConfig> config)
        {
            Event.Unsubscribe(this);
            _settings.Update(x => x.LastConnectionEndpoint = $"{netArgs.Ip}:{netArgs.Port}");
            _config = config;
            _netArgs = netArgs;
            _addressLabel.Text = netArgs.ShouldHost ? $"Hosting on port {netArgs.Port}" : $"Connecting to {netArgs.Ip}:{netArgs.Port}";
            _isConnecting = true;
            Event.Subscribe<GameConnected>(e =>
            {
                _isConnecting = false;
                _isConnected = true;
            }, this);
            if (_netArgs.ShouldHost && _netArgs.ShouldAutoLaunch)
                LaunchConnectingClient();
            if (!_netArgs.ShouldHost)
                Event.Subscribe<GameConfig>(StartGame, this);
        }

        private void StartGame(GameConfig e)
        {
            _config = e;
        }

        private void LaunchConnectingClient()
        {
            if (!(_isConnecting && _netArgs.ShouldHost))
                return; 
            
            var startInfo = new ProcessStartInfo
            {
                Arguments = $"{_netArgs.Ip} {_netArgs.Port}",
                FileName = Assembly.GetExecutingAssembly().Location
            };
            Process.Start(startInfo);
        }
    }
}
