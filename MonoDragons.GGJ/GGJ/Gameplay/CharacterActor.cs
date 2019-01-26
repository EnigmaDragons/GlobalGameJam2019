using MonoDragons.Core;
using MonoDragons.Core.EventSystem;

namespace MonoDragons.GGJ.Gameplay
{
    public class CharacterActor
    {
        private readonly CharacterState _state;

        public CharacterActor(CharacterState state)
        {
            _state = state;
            Event.Subscribe<PlayerDamaged>(OnDamaged, this);
        }

        private void OnDamaged(PlayerDamaged e)
        {
            if (_state.Controller == e.Target)
                _state.HP -= e.Amount;
        }
    }
}
