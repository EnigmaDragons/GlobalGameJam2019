using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Data;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MonoDragons.GGJ.UiElements.Events;

namespace MonoDragons.GGJ.Gameplay
{
    public class HandView : IVisualAutomatonControl
    {
        public ClickUIBranch Branch { get; private set; }

        private readonly GameData _data;
        private CharacterState State => _data[_player];
        private readonly List<CardView> _cards = new List<CardView>();
        private readonly List<bool> _cardIsPlayable = new List<bool>();
        private readonly List<Vector2> _positions = new List<Vector2>();
        private readonly Player _player;
        private readonly UiImage _chains = new UiImage { Image = "UI/card-chains", Transform = new Transform2(new Vector2(0, 0), new Size2(CardView.WIDTH, CardView.HEIGHT))};
        
        private float _totalMovementMs = 12000f;
        private float _elapsedMs = 0f;
        private float _from;
        private float _destination;
        private float _currentX = UI.OfScreenWidth(1.6f);
        private bool _isMoving;
        private Action _onArrived = () => { };

        public HandView(Player player, GameData data)
        {
            _player = player;
            _data = data;
            Branch = new ClickUIBranch("Hand", 1);
            Event.Subscribe<CardSelected>(OnCardSelected, this);
            Event.Subscribe<HandDrawn>(OnHandDrawn, this);
        }

        private void OnHandDrawn(HandDrawn e)
        {
            if (e.Player == State.Player)
                AddCards(e.Cards);
        }

        private void OnCardSelected(CardSelected e)
        {
            if (e.Player == State.Player)
                DiscardAll();
        }

        private void AddCards(List<int> cards)
        {
            foreach (var id in cards)
            {
                var cardView = Cards.Create(_data.AllCards[id]);
                var isPlayable = !State.Cards.UnplayableTypes.Contains(cardView.State.Type);
                _cards.Add(cardView);
                _cardIsPlayable.Add(isPlayable);
                _positions.Add(new Vector2(-2000));
            }

            Move(UI.OfScreenWidth(1.6f), 0, TimeSpan.FromMilliseconds(1200), () =>
            {
                for(var i = 0; i < _cards.Count; i++)
                {
                    var idx = i;
                    var pos = Position(i);
                    var cardView = Cards.Create(_data.AllCards[cards[i]]);
                    var isPlayable = !State.Cards.UnplayableTypes.Contains(cardView.State.Type);
                    if (isPlayable)
                        Branch.Add(new SimpleClickable(new Rectangle(pos.ToPoint(), new Point(CardView.WIDTH, CardView.HEIGHT)), () => CardSelected(idx)));
                }
            });
        }

        private void CardSelected(int cardIndex)
        {
            Event.Publish(new CardSelected(_cards[cardIndex].Id, _player));
        }

        private Vector2 Position(int index)
        {
            const int xMargin = CardView.WIDTH / 5;
            const int width = CardView.WIDTH + xMargin;
            const int height = CardView.HEIGHT;
            const int xOff = 80;
            const int yOff = 880;
            const bool useFanOutEffect = false;
            var fanOutFactor = useFanOutEffect ? Math.Abs(_currentX - _from) / Math.Abs(_from - _destination) : 1.0f;
            return new Vector2(xOff + _currentX + (index * width) * (fanOutFactor), yOff - height);
        }
        
        public void Draw(Transform2 parentTransform)
        {
            for (var i = 0; i < _positions.Count; i++)
            {
                var pos = new Transform2(_positions[i]);
                _cards[i].Draw(pos);
                if (!_cardIsPlayable[i])
                    _chains.Draw(pos);
            }
        }

        private void DiscardAll()
        {
            Branch.ClearElements();
            Move(0, -UI.OfScreenWidth(0.5f), TimeSpan.FromMilliseconds(1500), () =>
            {
                _positions.Clear();
                _cards.Clear();
                _cardIsPlayable.Clear();
            });
        }

        private void Move(int fromX, int toX, TimeSpan duration, Action onFinished)
        {
            _from = fromX;
            _destination = toX;
            _elapsedMs = 0;
            _totalMovementMs = (float)duration.TotalMilliseconds;
            _onArrived = onFinished;
            _isMoving = true;
            Event.Publish(new AnimationStarted("Move Cards"));
        }
        
        public void Update(TimeSpan delta)
        {
            if (_isMoving)
            {
                for (var i = 0; i < _cards.Count; i++)
                    _positions[i] = Position(i);
                _elapsedMs += (float)delta.TotalMilliseconds;
                _currentX = MathHelper.Lerp(_from, _destination, _elapsedMs / _totalMovementMs);
            }
            if (_isMoving && (Math.Abs(_currentX - _destination) < 5f))
            {
                _isMoving = false;
                _currentX = _destination;
                _onArrived();
                Event.Publish(new AnimationEnded());
            }
        }
    }
}
