using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.Network;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public sealed class WaitingForConnectionScene : ClickUiScene
    {
        private readonly string _message;
        private readonly NetworkArgs _netArgs;
        private readonly string _ip;
        private readonly int _port;
        private readonly bool _isHost;

        public WaitingForConnectionScene(string message, NetworkArgs netArgs)
        {
            _message = message;
            _netArgs = netArgs;
            _ip = netArgs.Ip.ToString();
            _port = netArgs.Port;
            _isHost = netArgs.ShouldHost;
        }

        public override void Init()
        {
            Add(new Label { Text = "Waiting For Connection", Transform = new Transform2(new Vector2(0, 0), new Size2(400, 60)) });
            Add(new Label { Text = _message, Transform = new Transform2(new Vector2(0, 60), new Size2(400, 60)) });
            Add(Buttons.Text("Cancel", new Point(40, 200), () => {
                Multiplayer.Disconnect();
                Scene.NavigateTo(new LobbyScene(new NetworkArgs(new string[] {})));
            }));

            Input.On(Control.Menu, LaunchConnectingClient);
            if (_isHost)
                Event.Subscribe<GameConnectionEstablished>(HostAsRandom, this);
            if (_isHost && _netArgs.ShouldAutoLaunch)
                LaunchConnectingClient();
            if (!_isHost)
                Event.Subscribe<RoleSelected>(r => Scene.NavigateTo(new GameScene(r.Role == Player.Cowboy ? Player.House : Player.Cowboy)), this);
        }

        private void LaunchConnectingClient()
        {
            var startInfo = new ProcessStartInfo
            {
                Arguments = $"{_ip} {_port}",
                FileName = Assembly.GetExecutingAssembly().Location
            };
            Process.Start(startInfo);
        }

        private void HostAsRandom(GameConnectionEstablished _)
        {
            var isHouse = Rng.Bool() ? Player.Cowboy : Player.House;
            Event.Publish(new RoleSelected(isHouse));
            Scene.NavigateTo(new GameScene(isHouse));
        }
    }
}