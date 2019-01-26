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
        private bool hasMadeSelection;

        public override void Init()
        {
            Add(new LevelBackground("House/level1"));
            Add(new BattleHud());
            Add(new Cowboy());
            Add(new Bed());
            Add(new Label { Text = "waiting for enemy", Transform = new Transform2(new Vector2(0, 0), new Size2(1600, 500)),
                IsVisible = () => hasMadeSelection });
            var deck = new Deck(new Card(), new Card(), new Card());
            hand = new Hand(deck.DrawCards(3));
            Add(hand);
            ClickUi.Add(hand.ClickUiBranch);
            Event.Subscribe(EventSubscription.Create<CardSelected>(CardSelected, this));
        }

        private void CardSelected(CardSelected obj)
        {
            hasMadeSelection = true;
            ClickUi.Remove(hand.ClickUiBranch);
        }
    }
}
