using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoDragons.Core;
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
        private Hand _hand;        
        private CardRevealer _cowboyRevealer;
        private CardRevealer _houseRevealer;
        private Deck _deck;

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
                IsVisible = () => !(_houseRevealer.Card.HasValue && _cowboyRevealer.Card.HasValue) });
            _cowboyRevealer = new CardRevealer(new Vector2(400, 350), !isHouse);
            Add(_cowboyRevealer);
            _houseRevealer = new CardRevealer(new Vector2(1200, 350), isHouse);
            Add(_houseRevealer);
            var deck = _player == Player.Cowboy 
                ? new Deck(Cards.GetCardById(1), Cards.GetCardById(2), Cards.GetCardById(3)) 
                : new Deck(Cards.GetCardById(4), Cards.GetCardById(5), Cards.GetCardById(6));
            _hand = new Hand(_player, deck.DrawCards(3));
            Add(_hand);
            var topHud = new BattleTopHud(_player, gameData);
            Add(topHud);
            ClickUi.Add(_hand.Branch);
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

            Event.Subscribe(EventSubscription.Create<CardSelected>(CardSelected, this));
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            if (!e.IsGameOver)
                return;

            ClickUi.Remove(_hand.Branch);
        }

        private void CardSelected(CardSelected selection)
        {
            if (selection.Player == Player.House)
                _houseRevealer.Card = new Optional<Card>(Cards.GetCardById(selection.CardId));
            else
                _cowboyRevealer.Card = new Optional<Card>(Cards.GetCardById(selection.CardId));
            if (selection.Player == _player)
                _hand.Empty();
            if (_cowboyRevealer.Card.HasValue && _houseRevealer.Card.HasValue)
            {
                _cowboyRevealer.IsRevealed = true;
                _houseRevealer.IsRevealed = true;
                Event.Publish(new AllCardsSelected { CowboyCard = _cowboyRevealer.Card.Value.Id, HouseCard = _houseRevealer.Card.Value.Id });
            }
        }

        private void StartNewTurn()
        {
            _houseRevealer.IsRevealed = _player == Player.House;
            _cowboyRevealer.IsRevealed = _player == Player.Cowboy;
            _houseRevealer.Card = new Optional<Card>();
            _cowboyRevealer.Card = new Optional<Card>();
            _hand.AddCards(_deck.DrawCards(2));
        }
    }
}
