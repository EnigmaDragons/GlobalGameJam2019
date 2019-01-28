using MonoDragons.Core.AudioSystem;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Gameplay;
using MonoDragons.GGJ.Gameplay.Events;
using MonoDragons.GGJ.UiElements.Events;

namespace MonoDragons.GGJ
{
    public class SoundEffectProcessor
    {
        private readonly Sound _cardSelcted = Sound.SoundEffect("Sounds\\ChooseCard.wav");
        private readonly Sound _applianceDeath = Sound.SoundEffect("Sounds\\Explosion.wav");
        private readonly Sound _cowboyHurt = Sound.SoundEffect("Sounds\\CowboyHurt.wav");
        private Player _player;

        public SoundEffectProcessor(Player player)
        {
            _player = player;
            Event.Subscribe<CardSelected>(OnCardSelected, this);
            Event.Subscribe<PlayerDamaged>(OnPlayerDamaged, this);
            Event.Subscribe<PlayerDefeated>(OnPlayerDefeated, this);
        }

        private void OnCardSelected(CardSelected e)
        {
            if (e.Player == _player)
                _cardSelcted.Play();
        }

        private void OnPlayerDamaged(PlayerDamaged e)
        {
            if (e.Target == Player.Cowboy && !_cowboyHurt.IsPlaying)
                _cowboyHurt.Play();
        }

        private void OnPlayerDefeated(PlayerDefeated e)
        {
            if (e.Winner == Player.Cowboy)
                _applianceDeath.Play();
        }
    }
}
