using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDragons.Core;
using MonoDragons.Core.Animations;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.Gameplay.Events;
using MonoDragons.GGJ.UiElements.Events;

namespace MonoDragons.GGJ.UiElements
{
    public class CardFightView : IVisualAutomaton
    {
        private readonly IconView _attackIcon;
        private bool _attackDoneMoving = false;
        private readonly IconView _defendIcon;
        private bool _defendIconDoneMoving = false;
        private readonly IAnimation _attackIconAnimation;
        private readonly IAnimation _defendIconAnimation;
        private readonly bool _isFlipped;
        private TimerTask _attackTimer;
        private TimerTask _defendTimer;
        private int _animationsPlaying = 0;
        private Action _onFinished;

        public CardFightView(CardView card, CharacterState state, CharacterState opponentState, bool isFlipped)
        {
            _attackIcon = IconView.Attack(isFlipped, card.State.PredictedDamage, opponentState.IncomingDamage - card.State.PredictedDamage, opponentState.DamageTakenMultipliers);
            _defendIcon = IconView.Defend(isFlipped, card.State.PredictedBlock, state.AvailableBlock - card.State.PredictedBlock, state.BlockRecievedMultiplier);
            var second = (CardView.WIDTH + 100) / 2 + 20;
            var first = second + 120;
            _attackIconAnimation = new SinglePositionTraverseAnimation(_attackIcon, new Vector2(isFlipped ? -first : first, 0), TimeSpan.FromSeconds(1), TimeSpan.Zero);
            _defendIconAnimation = new SinglePositionTraverseAnimation(_defendIcon, new Vector2(isFlipped ? -second : second, 0), TimeSpan.FromSeconds(1), TimeSpan.Zero);
            _isFlipped = isFlipped;
            Event.Subscribe<CardResolutionBegun>(e => _onFinished(), this);
        }

        public void Start(Action onFinished)
        {
            _onFinished = onFinished;
            Event.Publish(new AnimationStarted("Attack Shown"));
            _animationsPlaying++;
            _attackIconAnimation.Start(() =>
            {
                _attackIcon.Location = new Transform2(new Vector2(_isFlipped ? -240 : CardView.WIDTH + 140, (CardView.HEIGHT - 100) / 2));
                _attackDoneMoving = true;
                _attackIcon.Animate(() =>
                {
                    _attackTimer = new TimerTask(() =>
                    {
                        _animationsPlaying--;
                        Event.Publish(new AnimationEnded("Attack Shown"));
                    }, 2000, false);
                });
            });
            Event.Publish(new AnimationStarted("Defend Shown"));
            _animationsPlaying++;
            _defendIconAnimation.Start(() =>
            {
                _defendIcon.Location = new Transform2(new Vector2(_isFlipped ? -120 : CardView.WIDTH + 20, (CardView.HEIGHT - 100) / 2));
                _defendIconDoneMoving = true;
                _defendIcon.Animate(() =>
                {
                    _defendTimer = new TimerTask(() =>
                    {
                        _animationsPlaying--;
                        Event.Publish(new AnimationEnded("Defend Shown"));
                    }, 2000, false);
                });
            });
        }

        public void Update(TimeSpan delta)
        {
            _attackIconAnimation.Update(delta);
            _defendIconAnimation.Update(delta);
            _attackTimer?.Update(delta);
            _defendTimer?.Update(delta);
            _attackIcon.Update(delta);
            _defendIcon.Update(delta);
        }

        public void Draw(Transform2 parentTransform)
        {
            _attackIconAnimation.Draw(parentTransform);
            _defendIconAnimation.Draw(parentTransform);
            if (_attackDoneMoving)
                _attackIcon.Draw(parentTransform);
            if (_defendIconDoneMoving)
                _defendIcon.Draw(parentTransform);
        }
    }

    public class IconView : IVisualAutomaton
    {
        public static IconView Attack(bool isFlipped, int number, int bonus, List<MultiplierType> multipliers) 
            => new IconView(new Size2(100, 100), "attack", new Vector2((CardView.WIDTH - 100) / 2, (CardView.HEIGHT - 100) / 2), isFlipped, number, bonus, multipliers);
        public static IconView Defend(bool isFlipped, int number, int bonus, List<MultiplierType> multipliers) 
            => new IconView(new Size2(100, 100), "block", new Vector2((CardView.WIDTH - 100) / 2, (CardView.HEIGHT - 100) / 2), isFlipped, number, bonus, multipliers);

        private readonly Size2 _size;
        private readonly UiImage _icon;
        private int _number;
        private int _bonus;
        private readonly List<MultiplierType> _multipliers;

        private int _displayY;
        private string _displayText = "";
        private double _remainingMs;
        private Action _onfinished = () => {};

        public Transform2 Location { get; set; }

        public IconView(Size2 size, string icon, Vector2 location, bool isFlipped, int number, int bonus, List<MultiplierType> multipliers)
        {
            _size = size;
            _icon = new UiImage { Image = "UI/" + icon, Effects = isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None };
            Location = new Transform2(location);
            _number = number;
            _bonus = bonus;
            _multipliers = multipliers.ToList();
        }

        public void Animate(Action onFinished)
        {
            if (_bonus != 0)
            {
                _remainingMs = 2000;
                _displayText = _bonus > 0 ? "+" + _bonus : _bonus.ToString();
                _onfinished = () =>
                {
                    _number += _bonus;
                    _bonus = 0;
                    Animate(onFinished);
                };
            }
            else if (_multipliers.Any())
            {
                var multiplier = _multipliers.First();
                _remainingMs = 2000;
                if (multiplier == MultiplierType.Zero)
                    _displayText = "x0";
                if (multiplier == MultiplierType.Half)
                    _displayText = "1/2";
                if (multiplier == MultiplierType.Double)
                    _displayText = "x2";
                _onfinished = () =>
                {
                    _number *= (int) ((int) multiplier * 0.5m);
                    _multipliers.Remove(multiplier);
                    Animate(onFinished);
                };
            }
            else
            {
                _displayText = "";
                onFinished();
            }
        }

        public void Update(TimeSpan delta)
        {
            if (_remainingMs == 0)
                return;
            _remainingMs -= delta.TotalMilliseconds;
            if (_remainingMs <= 0)
            {
                _remainingMs = 0;
                _onfinished();
            }
            _displayY = (int)(Location.Location.Y - 10 - _remainingMs / 100);
        }

        public void Draw(Transform2 parentTransform)
        {           
            var transform = parentTransform + Location + new Transform2(_size);
            _icon.Draw(transform);
            UI.DrawTextCentered(_number.ToString(), transform.ToRectangle(), Color.White);
            UI.DrawTextCentered(_displayText, new Rectangle((int)transform.Location.X, _displayY + (int)parentTransform.Location.Y, transform.Size.Width, transform.Size.Height), Color.White);
        }
    }
}
