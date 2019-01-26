﻿using System;
using Microsoft.Xna.Framework;
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
        private Deck _deck;

        public GameScene(Player player)
        {
            _player = player;
        }

        public override void Init()
        {
            var g = new GameData();
            var isHouse = _player == Player.House;
            State<GameData>.Init(g);
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
            _deck = new Deck(new Card(), new Card(), new Card());
            _hand = new Hand(_player, _deck.DrawCards(3));
            Add(_hand);
            Add(new BattleTopHud(g));
            ClickUi.Add(_hand.Branch);
            Event.Subscribe(EventSubscription.Create<CardSelected>(CardSelected, this));
        }

        private void CardSelected(CardSelected selection)
        {
            if (selection.Player == Player.House)
                _houseRevealer.Card = new Optional<Card>(selection.Card);
            else
                _cowboyRevealer.Card = new Optional<Card>(selection.Card);
            if (selection.Player == _player)
                _hand.Empty();
            if (_cowboyRevealer.Card.HasValue && _houseRevealer.Card.HasValue)
            {
                _cowboyRevealer.IsRevealed = true;
                _houseRevealer.IsRevealed = true;
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
