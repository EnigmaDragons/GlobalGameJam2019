using MonoDragons.Core.EventSystem;

namespace MonoDragons.GGJ.Gameplay
{
    public class CharacterState
    {
        private readonly Player _controller;
        public int HP { get; private set; } = 50;

        public CharacterState(Player controller, int hp)
        {
            _controller = controller;
            HP = hp;
            EventSubscription.Create<PlayerDamaged>(OnDamaged, this);
        }

        private void OnDamaged(PlayerDamaged e)
        {
            if (_controller == e.Target)
                HP -= e.Amount;
        }
    }
}
