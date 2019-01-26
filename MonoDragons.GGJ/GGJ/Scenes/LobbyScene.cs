using System;
using System.Net;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Network;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public sealed class LobbyScene : ClickUiScene
    {
        private const string AppId = "Bed Dead Redemption";
        private static readonly Type[] NetTypes = { typeof(CardSelected), typeof(RoleSelected) };
        private readonly Label _hostEndpoint = new Label { Transform = new Transform2(new Vector2(260, 0), new Size2(200, 60)) };
        private readonly NetworkArgs _args;

        public LobbyScene(NetworkArgs args)
        {
            _args = args;
        }

        public override void Init()
        {
            Multiplayer.Disconnect();
            Add(Buttons.Text("Host", new Point(100, 160), BeginHostingGame));
            Add(Buttons.Text("Connect", new Point(100, 60), () => ConnectToGame(ParseURL(_hostEndpoint.Text))));
            Add(Buttons.Text("Play Solo", new Point(100, 260), CreateSinglePlayerGame));
            Add(new Label { Text = "Connect To:", Transform = new Transform2(new Vector2(0, 0), new Size2(400, 60)) });
            Add(_hostEndpoint);
            Add(new KeyboardTyping("127.0.0.1:4567").OutputTo(x => _hostEndpoint.Text = x));
            
            Logger.Write(_args);
            if (_args.ShouldAutoLaunch && !_args.ShouldHost)
                Add(new ActionAutomaton(() => ConnectToGame(_args.Ip, _args.Port)));
            if (_args.ShouldAutoLaunch && _args.ShouldHost)
                Add(new ActionAutomaton(BeginHostingGame));
        }

        private void CreateSinglePlayerGame()
        {
            Scene.NavigateTo(new GameScene(Player.Cowboy));
        }

        private void BeginHostingGame()
        {
            var ipEndpoint = ParseURL(_hostEndpoint.Text);
            Multiplayer.HostGame(AppId, ipEndpoint.Port, NetTypes);
            var networkArgs = new NetworkArgs(_args.ShouldAutoLaunch, true, ipEndpoint.Address.ToString(), ipEndpoint.Port);
            Scene.NavigateTo(new WaitingForConnectionScene($"Hosting on Port: {ipEndpoint.Port}", networkArgs));
        }

        private void ConnectToGame(IPEndPoint endPoint)
        {
            ConnectToGame(endPoint.Address.ToString(), endPoint.Port);
        }

        private void ConnectToGame(string ip, int port)
        {
            Multiplayer.JoinGame(AppId, ip, port, NetTypes);
            var networkArgs = new NetworkArgs(false, false, ip, port);
            Scene.NavigateTo(new WaitingForConnectionScene($"Connecting to host... {ip}:{port}", networkArgs));
        }
        
        private IPEndPoint ParseURL(string url)
        {
            Uri.TryCreate($"http://{url}", UriKind.Absolute, out Uri uri);
            return new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port);
        }
    }
}