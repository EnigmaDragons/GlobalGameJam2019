using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay.Events;

namespace MonoDragons.GGJ.Gameplay
{
    public class CharStatuses : IVisualAutomaton
    {
        private readonly Player _player;
        private List<IVisual> _statuses = new List<IVisual>();
        private List<IVisual> _nextStatuses = new List<IVisual>();
        private int vOffset = -62;
        
        public CharStatuses(Player player)
        {
            _player = player;
            Event.Subscribe<PlayerDefeated>(x => { Clear(); Clear(); }, this);
            Event.Subscribe<TurnFinished>(x => Clear(), this);
            Event.Subscribe<NextAttackEmpowered>(OnNextAttackEmpowered, this);
            Event.Subscribe<DamageTakenMultiplied>(OnDamageTakenMultiplied, this);
            Event.Subscribe<CardTypeLocked>(OnCardTypeLocked, this);
        }

        private void OnDamageTakenMultiplied(DamageTakenMultiplied e)
        {
            if (e.Target != _player)
                return;
            
            if (e.Type == MultiplierType.Double)
                _statuses.Add(Image("vulnerable"));
            else
                _statuses.Add(new Label { Text = $"x{e.Type.ToString()}", HorizontalAlignment = HorizontalAlignment.Left, Transform = new Transform2(new Size2(150, 50))});
        }

        private void Clear()
        {
            var next = _nextStatuses;
            _nextStatuses = new List<IVisual>();
            _statuses = next;
        }

        private void OnCardTypeLocked(CardTypeLocked e)
        {
            if (e.Target != _player) 
                return;
            
            if (e.Type == CardType.Attack)
                Queue("no-attack");
            else if (e.Type == CardType.Defend)
                Queue("no-defend");
            else
                _nextStatuses.Add(new Label {Text = $"No {e.Type}"});
        }

        private void OnNextAttackEmpowered(NextAttackEmpowered e)
        {
            if (e.Target == _player)
                Queue($"plus{e.Amount}");
        }

        public void Queue(string name)
        {
            _nextStatuses.Add(Image(name));
        }

        private UiImage Image(string name)
        {
            return new UiImage { Image = $"UI/{name}", Transform = new Transform2(new Size2(50, 50)) };
        }
            
        public void Update(TimeSpan delta)
        {
        }

        public void Draw(Transform2 parentTransform)
        {
            _statuses.ForEachIndex((x, i) => x.Draw(parentTransform + new Vector2(0, vOffset * i)));
        }
    }
}