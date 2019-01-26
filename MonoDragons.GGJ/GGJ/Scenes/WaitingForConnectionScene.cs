using System.Diagnostics;
using System.Reflection;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.Network;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public sealed class WaitingForConnectionScene : ClickUiScene
    {
        private readonly string _message;
        private readonly string _ip;
        private readonly int _port;

        public WaitingForConnectionScene(string message, string ip, int port)
        {
            _message = message;
            _ip = ip;
            _port = port;
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
            //Event.Subscribe<GameConnectionEstablished>(x => Scene.NavigateTo(new RockPaperScissorsGame()), this);
        }
    }
}