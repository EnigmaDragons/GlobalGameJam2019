using System;
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
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.Gameplay.Events;
using MonoDragons.GGJ.Scenes;
using MainMenuScene = MonoDragons.GGJ.Scenes.MainMenuScene;

namespace MonoDragons.Core
{
    public static class Program
    {
        public static readonly IErrorHandler ErrorHandler = new MessageBoxErrorHandler();
        public static readonly AppDetails AppDetails = new AppDetails(AppID.Value, "1.0", Environment.OSVersion.VersionString);

        [STAThread]
        static void Main(params string[] args)
        {
            var startingScene = "Logo";
            Error.Handle(() =>
            {
                var netArgs = new NetworkArgs(args);
                if (args.Length > 0)
                {
                    MasterVolume.Instance.MusicVolume = 0f;
                    MasterVolume.Instance.SoundEffectVolume = 0f;
                }
#if DEBUG
                MasterVolume.Instance.MusicVolume = 0f;
                DebugLogWindow.Launch();
                DebugLogWindow.Exclude(x => x.StartsWith("ActiveElementChanged"));
                DebugLogWindow.Exclude(x => x.StartsWith("DataStab"));
                netArgs = args.Length == 0 ? new NetworkArgs(true, true, "127.0.0.1", 4567) : netArgs;
                startingScene = "MainMenu";
#endif
                using (var game = new NeedlesslyComplexMainGame(AppDetails.Name, startingScene, new Display(1600, 900, false), SetupScene(netArgs), CreateKeyboardController(), ErrorHandler))
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
                { "Logo", () => new SimpleLogoScene("MainMenu", EnigmaDragonsResources.LogoImage) },
                { "MainMenu", () => new MainMenuScene(args) },
                { "Game", () => new GameScene(new GameConfig(Mode.SinglePlayer, Player.House, new GameData()), true) },
                { "UI", () => new UiTestScene()},
                { "Credits", () => new CreditsScene(Player.Cowboy)}
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
