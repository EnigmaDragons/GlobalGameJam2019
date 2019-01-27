using System;
using System.Diagnostics;
using System.Net;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Animations;
using MonoDragons.Core.AudioSystem;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Network;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.Text;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public sealed class MainMenuScene : ClickUiScene
    {
        private const string AppId = "Bed Dead Redemption";
        private static readonly Type[] NetTypes = { typeof(CardSelected), typeof(GameConfigured) };
        private Label _hostEndpoint;
        private readonly NetworkArgs _args;

        public MainMenuScene(NetworkArgs args)
        {
            _args = args;
        }

        public override void Init()
        {
            Sound.Music("The_Cowboy_Theme").Play();
            
            Add(new Sprite { Image = "Outside/desert_bg", Transform = new Transform2(UI.OfScreenSize(1.0f, 1.0f))});
            Add(new Sprite { Image = "Outside/desert_front", Transform = new Transform2(UI.OfScreenSize(1.0f, 1.0f))});
            Add(new Sprite { Image = "UI/title", Transform = new Transform2(new Vector2((1600 - 720) / 2, UI.OfScreenHeight(0.062f)), new Size2(720, 355))});

            Multiplayer.Disconnect();
            Add(Buttons.Wood("Host Game", UI.OfScreenSize(0.41f, 0.64f).ToPoint(), BeginHostingGame));
            Add(Buttons.Wood("Connect To Game", UI.OfScreenSize(0.41f, 0.75f).ToPoint(), () => ConnectToGame(ParseURL(_hostEndpoint.Text))));
            Add(Buttons.Wood("Play Solo", UI.OfScreenSize(0.41f, 0.86f).ToPoint(), CreateSinglePlayerGame));
            Add(new UiImage{ Image = "UI/wood-textbox", Transform = new Transform2(UI.OfScreen(0.40f, 0.51f), UI.OfScreenSize(0.20f, 0.12f))});
            _hostEndpoint =  new Label { Transform = new Transform2(UI.OfScreen(0.40f, 0.51f), UI.OfScreenSize(0.20f, 0.12f)), Font = DefaultFont.Medium};
            Add(_hostEndpoint);
            Add(new KeyboardTyping("127.0.0.1:4567").OutputTo(x => _hostEndpoint.Text = x));
            
            Add(new ImageButton("Images/logo", "Images/logo-hover", "Images/logo-press", new Transform2(UI.OfScreen(1.0f, 1.0f) - new Vector2(120, 120), new Size2(100, 100)), 
                () => Process.Start("https://www.enigmadragons.com")));
            
            Logger.Write(_args);
            if (_args.ShouldAutoLaunch && !_args.ShouldHost)
                Add(new ActionAutomaton(() => ConnectToGame(_args.Ip, _args.Port)));
            if (_args.ShouldAutoLaunch && _args.ShouldHost)
                Add(new ActionAutomaton(BeginHostingGame));
        }

        private void CreateSinglePlayerGame()
        {
            Scene.NavigateTo(new GameScene(new GameConfigured(Mode.SinglePlayer, Player.Cowboy, new GameData()), true));
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