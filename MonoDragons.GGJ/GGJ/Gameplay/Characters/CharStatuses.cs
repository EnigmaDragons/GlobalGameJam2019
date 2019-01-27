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
        private int vOffset = 60;
        
        public CharStatuses(Player player)
        {
            _player = player;
            Event.Subscribe<TurnFinished>(x => _statuses = new List<IVisual>(), this);
            Event.Subscribe<NextAttackEmpowered>(OnNextAttackEmpowered, this);
        }

        private void OnNextAttackEmpowered(NextAttackEmpowered e)
        {
            if (e.Target == _player)
                Add($"plus{e.Amount}");
        }

        public void Add(string status)
        {
            _statuses.Add(new UiImage { Image = $"UI/{status}", Transform = new Transform2(new Size2(62, 62))});
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