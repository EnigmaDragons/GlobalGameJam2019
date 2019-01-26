using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.UiElements;
using System;
using System.Collections.Generic;

namespace MonoDragons.GGJ.Scenes
{
    public class GameScene : ClickUiScene
    {
        private Hand hand;
        private bool isHouse;
        private CardRevealer cowboyRevealer;
        private CardRevealer houseRevealer;

        public GameScene(bool isHouse)
        {
            this.isHouse = isHouse;
        }

        public override void Init()
        {
            Add(new Label { Text = $"You are playing as " + (isHouse ? "house" : "cowboy"), Transform = new Transform2(new Vector2(0, 0), new Size2(1600, 800)) });
            Add(new LevelBackground("House/level1"));
            Add(new BattleHud());
            Add(new Cowboy());
            Add(new Bed());
            Add(new Label { Text = "waiting for enemy", Transform = new Transform2(new Vector2(0, 0), new Size2(1600, 500)),
                IsVisible = () => isHouse ? houseRevealer.Card.HasValue : cowboyRevealer.Card.HasValue });
            cowboyRevealer = new CardRevealer(new Vector2(400, 350), !isHouse);
            Add(cowboyRevealer);
            houseRevealer = new CardRevealer(new Vector2(1200, 350), isHouse);
            Add(houseRevealer);
            var deck = new Deck(new Card(), new Card(), new Card());
            hand = new Hand(isHouse, deck.DrawCards(3));
            Add(hand);
            ClickUi.Add(hand.Branch);
            Event.Subscribe(EventSubscription.Create<CardSelected>(CardSelected, this));
        }

        private void CardSelected(CardSelected selection)
        {
            if (selection.IsHouse)
                houseRevealer.Card = new Optional<Card>(selection.Card);
            else
                cowboyRevealer.Card = new Optional<Card>(selection.Card);
            if (cowboyRevealer.Card.HasValue && houseRevealer.Card.HasValue)
            {
                cowboyRevealer.IsRevealed = true;
                houseRevealer.IsRevealed = true;
            }
        }
    }
}
