using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core;
using MonoDragons.Core.AudioSystem;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Data;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.Gameplay.Events;
using MonoDragons.GGJ.OtherEvents;
using MonoDragons.GGJ.UiElements;
using MonoDragons.GGJ.UnnamedFolder;

namespace MonoDragons.GGJ.Scenes
{
    public class GameScene : ClickUiScene
    {
        private readonly Player _player;       
        private CardRevealer _cowboyRevealer;
        private CardRevealer _houseRevealer;
        private GameData _data;
        private bool _failedToSave = false;

        public GameScene(Player player)
        {
            _player = player;
        }

        public override void Init()
        {
#if DEBUG
            MasterVolume.Instance.MusicVolume = 0;
#endif
            Sound.Music("fight-it").Play();
            _data = new GameData();
            var isHouse = _player == Player.House;
            State<GameData>.Init(_data);
            Add(new PhaseTransitions(_data, _player));
            Add(new StateSynchronizer(_data, _player));
            Add(new Label { Text = $"You are playing as " + (isHouse ? "house" : "cowboy"), Transform = new Transform2(new Vector2(0, 0), new Size2(1600, 800)) });
            Add(new LevelBackground("House/level1"));
            Add(new BattleBackHud());
            Add(new Cowboy());
            Add(new Bed());
            Add(new Label { Text = "waiting for enemy", Transform = new Transform2(new Vector2(0, 0), new Size2(1600, 500)),
                IsVisible = () => !(_houseRevealer.Card.HasValue && _cowboyRevealer.Card.HasValue) });
            Add(new Label { Text = "failed to save", Transform = new Transform2(new Vector2(0, 0), new Size2(1600, 200)),
                IsVisible = () => _failedToSave });
            _cowboyRevealer = new CardRevealer(_player, Player.Cowboy, new Vector2(400, 350), !isHouse);
            Add(_cowboyRevealer);
            _houseRevealer = new CardRevealer(_player, Player.House, new Vector2(1200, 350), isHouse);
            Add(_houseRevealer);
            Add(_data[_player].Hand);
            new CharacterActor(_data.CowboyState);
            new CharacterActor(_data.HouseState);
            var topHud = new BattleTopHud(_player, _data);
            Add(topHud);
            ClickUi.Add(_data[_player].Hand.Branch);
            ClickUi.Add(topHud.Branch);

            // Temp
            Add(new ActionAutomaton(() =>
            {
                var keys = Keyboard.GetState();
                if (keys.IsKeyDown(Keys.C))
                    Event.Publish(new PlayerDefeated { Winner = Player.Cowboy, IsGameOver = true });
                if (keys.IsKeyDown(Keys.H))
                    Event.Publish(new PlayerDefeated { Winner = Player.House, IsGameOver = true });
                if (keys.IsKeyDown(Keys.Q))
                    Scene.NavigateTo("Lobby");
            }));

            Event.Subscribe<TurnFinished>(StartNewTurn, this);
            Event.Subscribe<FailedToSave>(_ => _failedToSave = true, this);
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            if (!e.IsGameOver)
                return;

            ClickUi.Remove(_data[_player].Hand.Branch);
        }

        private void StartNewTurn(TurnFinished e)
        {
            _data.CowboyState.Hand.AddCards(_data[_player].Deck.DrawCards(2));
            _data.HouseState.Hand.AddCards(_data[_player].Deck.DrawCards(2));
            Event.Publish(new TurnStarted());
        }
    }
}
