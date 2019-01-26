using MonoDragons.Core;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Gameplay.Events;

namespace MonoDragons.GGJ.Gameplay
{
    public class Character
    {
        private readonly CharacterState _state;
        private int _incomingDamage = 0;
        private int _availableBlock = 0;

        public Character(CharacterState state)
        {
            _state = state;
            Event.Subscribe<PlayerDamageProposed>(OnAttacked, this);
            Event.Subscribe<PlayerBlockProposed>(OnDefended, this);
            Event.Subscribe<CardResolutionBegun>(e => Resolve(), this);
        }

        private void OnAttacked(PlayerDamageProposed e)
        {
            if (e.Target == _state.Player)
                _incomingDamage = e.Amount;
        }

        private void OnDefended(PlayerBlockProposed e)
        {
            if (e.Target == _state.Player)
                _availableBlock = e.Amount;
        }

        private void Resolve()
        {
            if (_incomingDamage > _availableBlock)
            {
                var damage = _incomingDamage - _availableBlock;
                _state.HP -= damage;
                Event.Publish(new PlayerDamaged { Amount = damage, Target = _state.Player });
            }
            _incomingDamage = 0;
            _availableBlock = 0;
        }
    }
}
