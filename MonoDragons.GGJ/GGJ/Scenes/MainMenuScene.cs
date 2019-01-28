using System;
using System.Diagnostics;
using System.Net;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Animations;
using MonoDragons.Core.AudioSystem;
using MonoDragons.Core.Engine;
using MonoDragons.Core.IO;
using MonoDragons.Core.Network;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.Text;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.Gameplay.Events;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public sealed class MainMenuScene : ClickUiScene
    {
        private const string AppId = "Bed Dead Redemption";
        private static readonly Type[] NetTypes = { typeof(CardSelected), typeof(GameConfig), typeof(RematchRequested) };
        private readonly NetworkArgs _args;
        private readonly AppDataJsonIo _io;

        private Label _hostEndpoint;
        private ConnectingView _connecting;
        private readonly ClickUIBranch _menuBranch = new ClickUIBranch("MainMenuButtons", 1);
        private bool _isConnecting;
        
        public MainMenuScene(NetworkArgs args)
        {
            _args = args;
            _io = new AppDataJsonIo(AppId);
        }

        public override void Init()
        {
            Sound.Music("The_Cowboy_Theme").Play();
            
            Add(new SoundEffectProcessor(Player.Cowboy));
            Add(new Sprite { Image = "Outside/desert_bg", Transform = new Transform2(UI.OfScreenSize(1.0f, 1.0f))});
            Add(new Sprite { Image = "Outside/desert_front", Transform = new Transform2(UI.OfScreenSize(1.0f, 1.0f))});
            Add(new Sprite { Image = "UI/title", Transform = new Transform2(new Vector2((1600 - 720) / 2, UI.OfScreenHeight(0.062f)), new Size2(720, 355))});

            Multiplayer.Disconnect();
            AddMainMenuButton(Buttons.Wood("Host Game", UI.OfScreenSize(0.41f, 0.64f).ToPoint(), BeginHostingGame, () => !_isConnecting));
            AddMainMenuButton(Buttons.Wood("Connect To Game", UI.OfScreenSize(0.41f, 0.75f).ToPoint(), () => ConnectToGame(ParseURL(_hostEndpoint.Text)), () => !_isConnecting));
            AddMainMenuButton(Buttons.Wood("Play Solo", UI.OfScreenSize(0.41f, 0.86f).ToPoint(), CreateSinglePlayerGame, () => !_isConnecting));
            Add(new UiImage{ Image = "UI/wood-textbox", Transform = new Transform2(UI.OfScreen(0.40f, 0.51f), UI.OfScreenSize(0.20f, 0.12f)), IsActive = () => !_isConnecting});
            _hostEndpoint =  new Label { Transform = new Transform2(UI.OfScreen(0.40f, 0.51f), UI.OfScreenSize(0.20f, 0.12f)), Font = DefaultFont.Medium, IsVisible = () => !_isConnecting};
            Add(_hostEndpoint);
            Add(new KeyboardTyping("127.0.0.1:4567").OutputTo(x => _hostEndpoint.Text = x));
            _connecting = new ConnectingView(OnConnectingCancelled);
            _connecting.Init();
            Add(_connecting);
            Add(new ImageButton("Images/logo", "Images/logo-hover", "Images/logo-press", new Transform2(UI.OfScreen(1.0f, 1.0f) - new Vector2(120, 120), new Size2(100, 100)), 
                () => Process.Start("https://www.enigmadragons.com")));
            
            Logger.Write(_args);
            if (_args.ShouldAutoLaunch && !_args.ShouldHost)
                Add(new OnlyOnceAutomaton(() => ConnectToGame(_args.Ip, _args.Port)));
            if (_args.ShouldAutoLaunch && _args.ShouldHost)
                Add(new OnlyOnceAutomaton(BeginHostingGame));
        }

        private void AddMainMenuButton(ImageTextButton b)
        {
            Add(b);
            _menuBranch.Add(b);
        }

        private void OnConnectingCancelled()
        {
            _isConnecting = false;
            ClickUi.Add(_menuBranch);
            ClickUi.Remove(_connecting.Branch);
        }

        private void CreateSinglePlayerGame(Player player)
        {
            Scene.NavigateTo(new GameScene(new GameConfig(Mode.SinglePlayer, player, _io.Load<GameData>("Save")), true));
        }

        private void CreateSinglePlayerGame()
        {
            Scene.NavigateTo(new GameScene(new GameConfig(Mode.SinglePlayer, Player.Cowboy, new GameData()), true));
        }

        private void BeginHostingGame()
        {
            var ipEndpoint = ParseURL(_hostEndpoint.Text);
            Multiplayer.HostGame(AppId, ipEndpoint.Port, NetTypes);
            var networkArgs = new NetworkArgs(_args.ShouldAutoLaunch, true, ipEndpoint.Address.ToString(), ipEndpoint.Port);
            BeginConnecting(networkArgs, new GameConfig(Mode.MultiPlayer, Rng.Bool() ? Player.Cowboy: Player.House, new GameData()));
        }

        private void ConnectToGame(IPEndPoint endPoint)
        {
            ConnectToGame(endPoint.Address.ToString(), endPoint.Port);
        }

        private void ConnectToGame(string ip, int port)
        {
            Multiplayer.JoinGame(AppId, ip, port, NetTypes);
            var networkArgs = new NetworkArgs(false, false, ip, port);
            BeginConnecting(networkArgs, new Optional<GameConfig>());
        }

        private void BeginConnecting(NetworkArgs args, Optional<GameConfig> config)
        {
            _isConnecting = true;
            _connecting.Connect(args, config);
            ClickUi.Remove(_menuBranch);
            ClickUi.Add(_connecting.Branch);
        }
        
        private IPEndPoint ParseURL(string url)
        {
            Uri.TryCreate($"http://{url}", UriKind.Absolute, out Uri uri);
            return new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port);
        }
    }
}