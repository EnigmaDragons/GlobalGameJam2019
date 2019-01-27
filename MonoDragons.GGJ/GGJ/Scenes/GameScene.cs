using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core;
using MonoDragons.Core.AudioSystem;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.Network;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.Text;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.AI;
using MonoDragons.GGJ.Data;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.Gameplay.Events;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public class GameScene : ClickUiScene
    {
        private readonly Player _player;
        private readonly Mode _mode;
        private readonly GameData _data;
        private HandView _handView;
        private readonly bool _isHost;
        private bool _hasCowboyRequestedRematch = false;
        private bool _hasHouseRequestedRematch = false;
        private bool _gameEnded = false;

        public GameScene(GameConfigured config, bool isHost)
        {
            _isHost = isHost;
            _player = isHost ? config.HostRole : (config.HostRole == Player.Cowboy ? Player.House : Player.Cowboy);
            _mode = config.Mode;
            _data = config.StartingData;
        }

        public override void Init()
        {
            var rng = new GameRng(_data);
            Sound.Music("fight-it").Play();
            State<GameData>.Init(_data);
            var houseChars = new HouseCharacters(_data);
            
            Add(new DataSaver());
            Add(new LevelProgression(_data, houseChars));
            Add(new CardEffectProcessor(_data));
            Add(new NextTurnEffectProcessor(_data));
            Add(new CounterEffectProcessor(_data));
            Add(new StatusProcessor(_data));
            Add(new LastPlayedTypeLockProcessor(_data));
            Add(new Character(Player.Cowboy, _data));
            Add(new Character(Player.House, _data));
            Add(new PhaseTransitions(_data));
            Add(new LevelBackground(1));
            Add(new BattleBackHud(_player));
            Add(new Cowboy(_data.CurrentPhase));
            Add(houseChars);
            Add(new BattleTopHud(_player, _data));
            Add(new CardRevealer(_data, _player, Player.Cowboy, new Vector2(160, 880 - CardView.HEIGHT)));
            Add(new CardRevealer(_data, _player, Player.House, new Vector2(1600 - CardView.WIDTH - 160, 880 - CardView.HEIGHT)));
            _handView = new HandView(_player, _data, new Vector2(110, 880 - CardView.HEIGHT));
            Add(_handView);
            
            var cowboyCards = new PlayerCards(Player.Cowboy, _data, rng);
            Add(cowboyCards);
            var houseCards = new PlayerCards(Player.House, _data, rng);
            Add(houseCards);
            if (_mode == Mode.SinglePlayer)
                Add(new RandomCardAiPlayer(_player == Player.Cowboy ? Player.House : Player.Cowboy,
                    _data,
                    _player == Player.Cowboy ? houseCards : cowboyCards));
            if (_mode == Mode.MultiPlayer)
                Add(new ImageTextButton(
                    new Transform2(UI.OfScreenSize(0.41f, 0.25f).ToPoint().ToVector2(), UI.OfScreenSize(0.18f, 0.09f)),
                    () => Event.Publish(new RematchRequested(_player)),
                    "Rematch",
                    "UI/sign", "UI/sign-hover", "UI/sign-press",
                    () => _gameEnded && (_player == Player.House ? !_hasHouseRequestedRematch : !_hasCowboyRequestedRematch))
                {
                    Font = DefaultFont.Large,
                    TextColor = UiConsts.DarkBrown
                });
            
            // Temp
            Add(new ActionAutomaton(() =>
            {
                var keys = Keyboard.GetState();
                if (keys.IsKeyDown(Keys.C))
                    Event.Publish(new PlayerDefeated { LevelNumber = _data.CurrentLevel, Winner = Player.Cowboy, IsGameOver = false });
                if (keys.IsKeyDown(Keys.H))
                    Event.Publish(new PlayerDefeated { LevelNumber = _data.CurrentLevel, Winner = Player.House, IsGameOver = true });
                if (keys.IsKeyDown(Keys.F2))
                    Event.Publish(new NextLevelRequested { Level = 2 });
            }));

            Input.On(Control.Menu, () => Scene.NavigateTo(new MainMenuScene(new NetworkArgs())));
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);  
            Event.Subscribe<RematchRequested>(OnRematchRequested, this);
            Event.Subscribe<GameConfigured>(OnGameConfigured, this);
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
            Event.Subscribe<GameDisconnected>(OnDisconnected, this);
        }

        private void OnDisconnected(GameDisconnected obj)
        {
            if (_gameEnded)
                Scene.NavigateTo(new MainMenuScene(new NetworkArgs()));
        }

        private void OnGameConfigured(GameConfigured e)
        {
            if(!_isHost)
                Scene.NavigateTo(new GameScene(e, false));
        }

        private void OnRematchRequested(RematchRequested e)
        {
            if (e.Player == Player.Cowboy)
                _hasCowboyRequestedRematch = true;
            else
                _hasHouseRequestedRematch = true;
            if(_hasCowboyRequestedRematch && _hasHouseRequestedRematch && _isHost)
            {
                var config = new GameConfigured(Mode.MultiPlayer, Rng.Bool() ? Player.Cowboy : Player.House, new GameData());
                Event.Publish(config);
                Scene.NavigateTo(new GameScene(config, true));
            }
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            if (!e.IsGameOver)
                return;

            _gameEnded = true;
            ClickUi.Remove(_handView.Branch);
        }
    }
}
