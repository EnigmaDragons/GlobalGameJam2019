using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Graphics;
using MonoDragons.GGJ.Data;
using MonoDragons.GGJ.UiElements;

namespace MonoDragons.GGJ.Gameplay
{
    public class HouseCharacters : IVisualAutomaton
    {
        private readonly MustInit<IHouseChar> _char = new MustInit<IHouseChar>("House Current Char");
        private readonly Explosion _exp = new Explosion();
        private readonly DamageNumbersView _dmgView = new DamageNumbersView(Player.House);
        private readonly Vector2 _dmgViewOffset = new Vector2(120, 0);
        private readonly CharStatuses _statusView = new CharStatuses(Player.House);
        private readonly Vector2 _statusOffset = new Vector2(-45, 230);
        private readonly BobbingEffect _bobbing = new BobbingEffect();
        private bool _shouldShow;

        public HouseCharacters(GameData data)
        {
            _char.Init(Enemies.Create(data.CurrentEnemy));
            _shouldShow = data.CurrentPhase != Phase.Setup;
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
            Event.Subscribe<LevelSetup>(e => _shouldShow = true, this);
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            if (e.Winner != Player.House)
                _exp.Start(() => Initialized(new NoChar()));
        }

        public HouseCharacters Initialized(IHouseChar current)
        {
            _shouldShow = false;
            _char.Init(current);
            return this;
        }

        public void Update(TimeSpan delta)
        {
            _bobbing.Update(delta);
            _dmgView.Update(delta);
            _char.Get().Update(delta);
            _exp.Update(delta);
            _statusView.Update(delta);
        }

        public void Draw(Transform2 parentTransform)
        {
            if (!_shouldShow)
                return;
            
            _bobbing.Draw(_char.Get(), parentTransform);
            _statusView.Draw(parentTransform + _char.Get().Transform.Location + _statusOffset);
            _dmgView.Draw(parentTransform + _char.Get().Transform.Location + _dmgViewOffset);
            _exp.Draw(parentTransform + _char.Get().Transform.Location);
        }
    }
}