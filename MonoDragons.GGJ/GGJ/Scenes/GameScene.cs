using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core;
using MonoDragons.Core.AudioSystem;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.AI;
using MonoDragons.GGJ.Data;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public class GameScene : ClickUiScene
    {
        private readonly Player _player;
        private readonly Mode _mode;
        private CardRevealer _cowboyRevealer;
        private CardRevealer _houseRevealer;
        private readonly GameData _data;
        private HandView _handView;

        public GameScene(GameConfigured gameOptionSelected, bool isHost)
        {
            _player = isHost ? gameOptionSelected.HostRole : (gameOptionSelected.HostRole == Player.Cowboy ? Player.House : Player.Cowboy);
            _mode = gameOptionSelected.Mode;
            _data = gameOptionSelected.StartingData;
        }

        public override void Init()
        {
            var rng = new GameRng(_data);
            Sound.Music("fight-it").Play();
            State<GameData>.Init(_data);
            var houseChars = new HouseCharacters();
            Add(new LevelProgression(_data, houseChars));
            Add(new CardEffectProcessor(_data));
            Add(new NextTurnEffectProcessor());
            Add(new CounterEffectProcessor(_data));
            Add(new StatusProcessor(_data));
            Add(new Character(Player.Cowboy, _data));
            Add(new Character(Player.House, _data));
            var cowboyCards = new PlayerCards(Player.Cowboy, _data, rng);
            Add(cowboyCards);
            var houseCards = new PlayerCards(Player.House, _data, rng);
            Add(houseCards);
            if (_mode == Mode.SinglePlayer)
                Add(new RandomCardAiPlayer(_player == Player.Cowboy ? Player.House : Player.Cowboy,
                    _data,
                    _player == Player.Cowboy ? houseCards : cowboyCards));

            Add(new PhaseTransitions(_data));
            Add(new LevelBackground("House/level1"));
            Add(new BattleBackHud());
            Add(new Cowboy());
            Add(houseChars);
            Add(new Label { Text = "waiting for enemy", Transform = new Transform2(new Vector2(0, 0), new Size2(1600, 500)),
                IsVisible = () => _player == Player.House
                    ? _houseRevealer.Card.HasValue && !_cowboyRevealer.Card.HasValue
                    : _cowboyRevealer.Card.HasValue && !_houseRevealer.Card.HasValue});
            _cowboyRevealer = new CardRevealer(_player, Player.Cowboy, new Vector2(400, 350));
            Add(_cowboyRevealer);
            _houseRevealer = new CardRevealer(_player, Player.House, new Vector2(1200, 350));
            Add(_houseRevealer);
            _handView = new HandView(_player, _data);
            Add(_handView);
            Add(new BattleTopHud(_player, _data));

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
                if (keys.IsKeyDown(Keys.F2))
                    Event.Publish(new NextLevelRequested { Level = 1 });
            }));
            
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            if (!e.IsGameOver)
                return;

            ClickUi.Remove(_handView.Branch);
        }
    }
}
