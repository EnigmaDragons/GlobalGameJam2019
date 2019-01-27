using System.Diagnostics;
using System.Reflection;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.IO;
using MonoDragons.Core.Network;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay.Events;
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
        private readonly Optional<GameConfigured> _config;

        public WaitingForConnectionScene(string message, NetworkArgs netArgs)
        {
            _message = message;
            _netArgs = netArgs;
            _ip = netArgs.Ip.ToString();
            _port = netArgs.Port;
            _isHost = false;
            _config = new Optional<GameConfigured>();
            new AppDataJsonIo("GGJ2019").Save("GamePrefs", netArgs.Ip.ToString() + ":" + netArgs.Port);
        }

        public WaitingForConnectionScene(string message, NetworkArgs netArgs, GameConfigured config)
        {
            _message = message;
            _netArgs = netArgs;
            _ip = netArgs.Ip.ToString();
            _port = netArgs.Port;
            _isHost = true;
            _config = new Optional<GameConfigured>(config);
            new AppDataJsonIo("GGJ2019").Save("GamePrefs", netArgs.Ip.ToString() + ":" + netArgs.Port);
        }

        public override void Init()
        {
            Add(new Label { Text = "Waiting For Connection", Transform = new Transform2(new Vector2(0, 0), new Size2(400, 60)) });
            Add(new Label { Text = _message, Transform = new Transform2(new Vector2(0, 60), new Size2(400, 60)) });
            Add(Buttons.Text("Cancel", new Point(40, 200), () => {
                Multiplayer.Disconnect();
                Scene.NavigateTo(new MainMenuScene(new NetworkArgs(new string[] {})));
            }));

            Input.On(Control.Menu, LaunchConnectingClient);
            if (_isHost)
                Event.Subscribe<GameConnected>(Host, this);
            if (_isHost && _netArgs.ShouldAutoLaunch)
                LaunchConnectingClient();
            if (!_isHost)
                Event.Subscribe<GameConfigured>(e => Scene.NavigateTo(new GameScene(e, false)), this);
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

        private void Host(GameConnected _)
        {
            Event.Publish(_config.Value);
            Scene.NavigateTo(new GameScene(_config.Value, true));
        }
    }
}