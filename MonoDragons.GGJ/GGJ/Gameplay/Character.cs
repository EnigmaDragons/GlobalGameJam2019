using MonoDragons.Core;
using MonoDragons.Core.EventSystem;

namespace MonoDragons.GGJ.Gameplay
{
    public class Character
    {
        private readonly CharacterState _state;

        public Character(CharacterState state)
        {
            _state = state;
            Event.Subscribe<PlayerDamaged>(OnDamaged, this);
        }

        private void OnDamaged(PlayerDamaged e)
        {
            if (_state.Player == e.Target)
                _state.HP -= e.Amount;
        }
    }
}
