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
        private readonly string _ip;
        private readonly int _port;
        private readonly bool _isHost;

        public WaitingForConnectionScene(string message, string ip, int port, bool isHost)
        {
            _message = message;
            _ip = ip;
            _port = port;
            _isHost = isHost;
        }

        public override void Init()
        {
            Add(new Label { Text = "Waiting For Connection", Transform = new Transform2(new Vector2(0, 0), new Size2(400, 60)) });
            Add(new Label { Text = _message, Transform = new Transform2(new Vector2(0, 60), new Size2(400, 60)) });
            Add(Buttons.Text("Cancel", new Point(40, 200), () => {
                Multiplayer.Disconnect();
                Scene.NavigateTo(new LobbyScene(new NetworkArgs(new string[] {})));
            }));


            Input.On(Control.Menu, () =>
            {
                var startInfo = new ProcessStartInfo
                {
                    Arguments = $"{_ip} {_port}",
                    FileName = Assembly.GetExecutingAssembly().Location
                };
                Process.Start(startInfo);
            });
            if (_isHost)
                Event.Subscribe<GameConnectionEstablished>(HostAsRandom, this);
            else
                Event.Subscribe<RoleSelected>(r => Scene.NavigateTo(new GameScene(!r.IsPlayingAsHouse)), this);
        }

        private void HostAsRandom(GameConnectionEstablished _)
        {
            var isHouse = Rng.Bool();
            Event.Publish(new RoleSelected(isHouse));
            Scene.NavigateTo(new GameScene(isHouse));
        }
    }
}