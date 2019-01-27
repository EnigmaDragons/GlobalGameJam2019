﻿using System;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core.AudioSystem;
using MonoDragons.Core.Development;
using MonoDragons.Core.EngimaDragons;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Errors;
using MonoDragons.Core.Examples;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.Memory;
using MonoDragons.Core.Network;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;
using MonoDragons.GGJ;
using MonoDragons.GGJ.Scenes;

namespace MonoDragons.Core
{
    public static class Program
    {
        public static readonly IErrorHandler ErrorHandler = new MessageBoxErrorHandler();
        public static readonly AppDetails AppDetails = new AppDetails("MonoDragons GGJ", "0.0", Environment.OSVersion.VersionString);

        [STAThread]
        static void Main(params string[] args)
        {
            Error.Handle(() =>
            {
                var netArgs = new NetworkArgs(args);
                if (args.Length > 0)
                {
                    MasterVolume.Instance.MusicVolume = 0;
                    MasterVolume.Instance.SoundEffectVolume = 0;
                }
#if DEBUG
                MasterVolume.Instance.MusicVolume = 0;
                DebugLogWindow.Launch();
                DebugLogWindow.Exclude(x => x.StartsWith("ActiveElementChanged"));
                netArgs = args.Length == 0 ? new NetworkArgs(true, true, "127.0.0.1", 4567) : netArgs;
#endif
                using (var game = new NeedlesslyComplexMainGame(AppDetails.Name, "Game", new Display(1600, 900, false), SetupScene(netArgs), CreateKeyboardController(), ErrorHandler))
                    game.Run();
            }, ErrorHandler.Handle);
        }

        private static IScene SetupScene(NetworkArgs args)
        {
            var currentScene = new CurrentScene();
            Scene.Init(new CurrentSceneNavigation(currentScene, CreateSceneFactory(args),  
                AudioPlayer.Instance.StopAll, 
                Resources.Unload));
            return new HideViewportExternals(currentScene);
        }

        private static SceneFactory CreateSceneFactory(NetworkArgs args)
        {
            return new SceneFactory(new Map<string, Func<IScene>>
            {
                { "Logo", () => new SimpleLogoScene("Lobby", EnigmaDragonsResources.LogoImage) },
                { "Lobby", () => new LobbyScene(args) },
                { "Game", () => new GameScene(Player.Cowboy, Mode.SinglePlayer) },
                { "UI", () => new UiTestScene()}
            });
        }

        private static IController CreateKeyboardController()
        {
            return new KeyboardController(new Map<Keys, Control>
            {
                { Keys.Space, Control.Select },
                { Keys.Enter, Control.Start },
                { Keys.F1, Control.Menu },
                { Keys.V, Control.A },
                { Keys.O, Control.X }
            });
        }
    }
}
