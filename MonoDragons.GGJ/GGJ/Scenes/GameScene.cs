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
        private readonly GameData _data;
        private HandView _handView;

        public GameScene(GameConfigured config, bool isHost)
        {
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
            Add(new CardRevealer(_player, Player.Cowboy, new Vector2(160, 880 - CardView.HEIGHT)));
            Add(new CardRevealer(_player, Player.House, new Vector2(1600 - CardView.WIDTH - 160, 880 - CardView.HEIGHT)));
            _handView = new HandView(_player, _data, new Vector2(110, 880 - CardView.HEIGHT));
            Add(_handView);
            
            var cowboyCards = new PlayerCards(Player.Cowboy, _data, rng);
            Add(cowboyCards);
            var houseCards = new PlayerCards(Player.House, _data, rng);
            Add(houseCards);
            if (_mode == Mode.SinglePlayer)
                Add(new RandomCardAiPlayer(_player == Player.Cowboy ? Player.House : Player.Cowboy,
                    _data,
                    _player == Player.Cowboy ? cowboyCards : houseCards));

            // Temp
            Add(new ActionAutomaton(() =>
            {
                var keys = Keyboard.GetState();
                if (keys.IsKeyDown(Keys.C))
                    Event.Publish(new PlayerDefeated { LevelNumber = _data.CurrentLevel, Winner = Player.Cowboy, IsGameOver = false });
                if (keys.IsKeyDown(Keys.H))
                    Event.Publish(new PlayerDefeated { LevelNumber = _data.CurrentLevel, Winner = Player.House, IsGameOver = true });
                if (keys.IsKeyDown(Keys.Q))
                    Scene.NavigateTo("Lobby");
                if (keys.IsKeyDown(Keys.F2))
                    Event.Publish(new NextLevelRequested { Level = 2 });
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
