using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Scenes
{
    public class GameScene : ClickUiScene
    {
        private readonly Player _player;
        private Hand _hand;
        private CardRevealer _cowboyRevealer;
        private CardRevealer _houseRevealer;

        public GameScene(Player player)
        {
            _player = player;
        }

        public override void Init()
        {
            var gameData = new GameData();
            var isHouse = _player == Player.House;
            State<GameData>.Init(gameData);
            Add(new Label { Text = $"You are playing as " + (isHouse ? "house" : "cowboy"), Transform = new Transform2(new Vector2(0, 0), new Size2(1600, 800)) });
            Add(new LevelBackground("House/level1"));
            Add(new BattleBackHud());
            Add(new Cowboy());
            Add(new Bed());
            Add(new Label { Text = "waiting for enemy", Transform = new Transform2(new Vector2(0, 0), new Size2(1600, 500)),
                IsVisible = () => isHouse ? _houseRevealer.Card.HasValue : _cowboyRevealer.Card.HasValue });
            _cowboyRevealer = new CardRevealer(new Vector2(400, 350), !isHouse);
            Add(_cowboyRevealer);
            _houseRevealer = new CardRevealer(new Vector2(1200, 350), isHouse);
            Add(_houseRevealer);
            var deck = new Deck(new Card(), new Card(), new Card());
            _hand = new Hand(isHouse, deck.DrawCards(3));
            Add(_hand);
            Add(new BattleTopHud(_player, gameData));
            ClickUi.Add(_hand.Branch);

            // Temp
            Add(new ActionAutomaton(() =>
            {
                if (Keyboard.GetState().IsKeyDown(Keys.C))
                    Event.Publish(new PlayerDefeated { Winner = Player.Cowboy, IsGameOver = true });
                if (Keyboard.GetState().IsKeyDown(Keys.H))
                    Event.Publish(new PlayerDefeated { Winner = Player.House, IsGameOver = true });
            }));

            Event.Subscribe(EventSubscription.Create<CardSelected>(CardSelected, this));
        }

        private void CardSelected(CardSelected selection)
        {
            if (selection.IsHouse)
                _houseRevealer.Card = new Optional<Card>(selection.Card);
            else
                _cowboyRevealer.Card = new Optional<Card>(selection.Card);
            if (_cowboyRevealer.Card.HasValue && _houseRevealer.Card.HasValue)
            {
                _cowboyRevealer.IsRevealed = true;
                _houseRevealer.IsRevealed = true;
            }
        }
    }
}
