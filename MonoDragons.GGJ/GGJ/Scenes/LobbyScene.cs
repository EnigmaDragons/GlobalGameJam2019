using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.Network;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public sealed class LobbyScene : ClickUiScene
    {
        private const string AppId = "Scissors Paper Rock Example";
        private const int Port = 44559;
        private static readonly Type[] NetTypes = { };
        private readonly Label _hostEndpoint = new Label { Transform = new Transform2(new Vector2(260, 0), new Size2(200, 60)) };
        private readonly NetworkArgs _args;

        public LobbyScene(NetworkArgs args)
        {
            _args = args;
        }

        public override void Init()
        {
            Add(Buttons.Text("Host", new Point(100, 160), BeginHostingGame));
            Add(Buttons.Text("Connect", new Point(100, 60), () => ConnectToGame(_hostEndpoint.Text, Port)));
            Add(new Label { Text = "Connect To:", Transform = new Transform2(new Vector2(0, 0), new Size2(400, 60)) });
            Add(_hostEndpoint);
            Add(new KeyboardTyping("127.0.0.1").OutputTo(x => _hostEndpoint.Text = x));
            
            if (_args.ShouldConnect)
                ConnectToGame(_args.Ip, _args.Port);
        }

        private void BeginHostingGame()
        {
            Multiplayer.HostGame(AppId, Port, NetTypes);
            Scene.NavigateTo(new WaitingForConnectionScene($"Hosting on Port: {Port}", _hostEndpoint.Text, Port));
        }

        private void ConnectToGame(string ip, int port)
        {
            Multiplayer.JoinGame(AppId, _hostEndpoint.Text, Port, NetTypes);
            Scene.NavigateTo(new WaitingForConnectionScene($"Connecting to host... {_hostEndpoint.Text}:{Port}", _hostEndpoint.Text, Port));
        }
    }
}