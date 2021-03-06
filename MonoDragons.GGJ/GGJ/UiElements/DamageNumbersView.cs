﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Text;
using MonoDragons.Core.UserInterface;
using MonoDragons.GGJ.Gameplay;

namespace MonoDragons.GGJ.UiElements
{
    public class DamageNumbersView : IVisualAutomaton
    {
        private readonly Player _player;
        private static readonly TimeSpan DisplayDuration = TimeSpan.FromMilliseconds(1800);
        private static readonly TimeSpan DelayDuration = TimeSpan.FromMilliseconds(300);
        private readonly List<Tuple<TimeSpan, string>> _numbers = new List<Tuple<TimeSpan, string>>();
        private readonly List<Tuple<TimeSpan, string>> _delayed = new List<Tuple<TimeSpan, string>>();
        private bool _isDisplayingDamage;

        public DamageNumbersView(Player player)
        {
            _player = player;
            Event.Subscribe<PlayerDamaged>(OnPlayerDamaged, this);
        }

        private void OnPlayerDamaged(PlayerDamaged e)
        {
            if (e.Target == _player)
                Add(e.Amount.ToString());
        }

        private void Add(string text)
        {
            _delayed.Add(new Tuple<TimeSpan, string>(
                DelayDuration,
                text));
            _isDisplayingDamage = true;
        }

        public void Draw(Transform2 parentTransform)
        {
            var numbers = _numbers.ToList();
            for (var i = 0; i < numbers.Count; i++)
                UI.DrawText(numbers[i].Item2, parentTransform.Location + new Vector2(5, -34 - (i * 24)), Color.White, DefaultFont.Large);
        }

        public void Update(TimeSpan delta)
        {
            if (_isDisplayingDamage && _numbers.Count == 0 && _delayed.Count == 0)
                _isDisplayingDamage = false;
            
            var updated =  _delayed.ToList()
                .Select(x => new Tuple<TimeSpan, string>(x.Item1 - delta, x.Item2)).ToList();
            _delayed.Clear();
            _delayed.AddRange(updated.Where(x => x.Item1 > TimeSpan.Zero));
            
            var numbers = _numbers.ToList();
            _numbers.Clear();
            _numbers.AddRange(numbers
                .Select(x => new Tuple<TimeSpan, string>(x.Item1 - delta, x.Item2))
                .Where(i => i.Item1 > TimeSpan.Zero));
            _numbers.AddRange(updated
                .Where(x => x.Item1 <= TimeSpan.Zero)
                .Select(i => new Tuple<TimeSpan, string>(DisplayDuration, i.Item2)));
        }
    }
}