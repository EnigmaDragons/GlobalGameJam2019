using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
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
            Add(Buttons.Text("Connect", new Point(100, 60), () => ConnectToGame(ParseURL(_hostEndpoint.Text))));
            Add(Buttons.Text("Play Solo", new Point(100, 260), CreateSinglePlayerGame));
            Add(new Label { Text = "Connect To:", Transform = new Transform2(new Vector2(0, 0), new Size2(400, 60)) });
            Add(_hostEndpoint);
            Add(new KeyboardTyping("127.0.0.1:4567").OutputTo(x => _hostEndpoint.Text = x));
            
            Logger.Write(_args);
            if (_args.ShouldConnect)
                Add(new ActionAutomaton(() => ConnectToGame(_args.Ip, _args.Port)));
        }

        private void CreateSinglePlayerGame()
        {
            Scene.NavigateTo(new GameScene());
        }

        private void BeginHostingGame()
        {
            var ipEndpoint = ParseURL(_hostEndpoint.Text);
            Multiplayer.HostGame(AppId, ipEndpoint.Port, NetTypes);
            Scene.NavigateTo(new WaitingForConnectionScene($"Hosting on Port: {ipEndpoint.Port}", ipEndpoint.Address.ToString(), ipEndpoint.Port));
        }

        private void ConnectToGame(IPEndPoint endPoint)
        {
            ConnectToGame(endPoint.Address.ToString(), endPoint.Port);
        }

        private void ConnectToGame(string ip, int port)
        {
            Multiplayer.JoinGame(AppId, ip, port, NetTypes);
            Scene.NavigateTo(new WaitingForConnectionScene($"Connecting to host... {ip}:{port}", ip, port));
        }
        
        private IPEndPoint ParseURL(string url)
        {
            Uri.TryCreate($"http://{url}", UriKind.Absolute, out Uri uri);
            return new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port);
        }
    }
}