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
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public class GameScene : ClickUiScene
    {
        private readonly Player _player;       
        private CardRevealer _cowboyRevealer;
        private CardRevealer _houseRevealer;
        private GameData _data;
        private HandView _handView;

        public GameScene(Player player)
        {
            _player = player;
        }

        public override void Init()
        {
            Sound.Music("fight-it").Play();
            // TODO: Move Setup out of Scene
            _data = new GameData();
            SetupCharacters();
            Add(new PlayerCards(_player, _data[_player].Cards));
            Add(new CardEffectProcessor(_data));

            var isHouse = _player == Player.House;
            State<GameData>.Init(_data);
            Add(new PhaseTransitions(_data));
            Add(new Label { Text = $"You are playing as " + (isHouse ? "house" : "cowboy"), Transform = new Transform2(new Vector2(0, 0), new Size2(1600, 800)) });
            Add(new LevelBackground("House/level1"));
            Add(new BattleBackHud());
            Add(new Cowboy());
            Add(new Bed());
            Add(new Label { Text = "waiting for enemy", Transform = new Transform2(new Vector2(0, 0), new Size2(1600, 500)),
                IsVisible = () => !(_houseRevealer.Card.HasValue && _cowboyRevealer.Card.HasValue) });
            _cowboyRevealer = new CardRevealer(_player, Player.Cowboy, new Vector2(400, 350), !isHouse);
            Add(_cowboyRevealer);
            _houseRevealer = new CardRevealer(_player, Player.House, new Vector2(1200, 350), isHouse);
            Add(_houseRevealer);
            _handView = new HandView(_data, _data[_player]);
            Add(_handView);
            Add(new Character(_data.CowboyState));
            Add(new Character(_data.HouseState));
            Add(new LevelProgression(_data));
            var topHud = new BattleTopHud(_player, _data);
            Add(topHud);
            ClickUi.Add(_handView.Branch);
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
                if (keys.IsKeyDown(Keys.F2))
                    Event.Publish(new NextLevelRequested { Level = 1 });
            }));
            
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
            Event.Publish(new LevelSetup { CurrentLevel = _data.CurrentLevel });
            Event.Publish(new TurnStarted { TurnNumber = _data.CurrentTurn + 1 });
        }

        private void SetupCharacters()
        {
            _data.CowboyState = new CharacterState(Player.Cowboy, 10, 
                new PlayerCardsState(
                    CreateCard(CardName.CowboyPass),
                    CreateCard(CardName.CrackShot),
                    CreateCard(CardName.FanTheHammer),
                    CreateCard(CardName.GunsBlazing),
                    CreateCard(CardName.DuckAndCover)));

            _data.HouseState = new CharacterState(Player.House, 10,
                new PlayerCardsState(
                    CreateCard(CardName.HousePass),
                    CreateCard(CardName.Lazer),
                    CreateCard(CardName.WaterLeak),
                    CreateCard(CardName.ElectricShockSuperAttack)));
        }

        private CardState CreateCard(CardName cardName)
        {
            var result = new CardState { Id = GetNextCardId(), CardName = cardName };
            _data.AllCards[result.Id] = result;
            return result;
        }

        private int GetNextCardId()
        {
            var result = _data.CurrentCardId;
            _data.CurrentCardId++;
            return result;
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            if (!e.IsGameOver)
                return;

            ClickUi.Remove(_handView.Branch);
        }
    }
}
