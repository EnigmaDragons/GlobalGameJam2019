using System.Collections.Generic;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Gameplay.Events;

namespace MonoDragons.GGJ.Gameplay
{
    public class Character
    {
        private int _incomingDamage = 0;
        private int _availableBlock = 0;
        private List<object> _onNotDamaged = new List<object>();
        private readonly Player _player;
        private readonly GameData _data;
        private CharacterState State => _data[_player];
        private CharacterState OpponentState => _data[_player == Player.Cowboy ? Player.House : Player.Cowboy];

        public Character(Player player, GameData data)
        {
            _player = player;
            _data = data;
            Event.Subscribe<PlayerDamageProposed>(OnAttacked, this);
            Event.Subscribe<PlayerBlockProposed>(OnDefended, this);
            Event.Subscribe<OnNotDamagedEffectQueued>(OnNotDamagedEffectQueued, this);
            Event.Subscribe<CardSelected>(OnCardTypeSelected, this);
            Event.Subscribe<NextAttackEmpowered>(OnNextAttackEmpowered, this);
            Event.Subscribe<CardResolutionBegun>(e => Resolve(), this);
        }

        private void OnAttacked(PlayerDamageProposed e)
        {
            if (e.Target == _player)
                _incomingDamage += e.Amount;
        }

        private void OnDefended(PlayerBlockProposed e)
        {
            if (e.Target == _player)
                _availableBlock += e.Amount;
        }

        private void OnNotDamagedEffectQueued(OnNotDamagedEffectQueued e)
        {
            if (e.Target == _player)
                _onNotDamaged.Add(e.Event);
        }

        private void OnCardTypeSelected(CardSelected e)
        {
            if (OpponentState.NextAttackBonus > 0 && e.Player != _player && _data.AllCards[e.CardId].Type == CardType.Attack)
            {
                _incomingDamage += OpponentState.NextAttackBonus;
                OpponentState.NextAttackBonus = 0;
            }
        }

        private void OnNextAttackEmpowered(NextAttackEmpowered e)
        {
            if (e.Target == _player)
                State.NextAttackBonus += e.Amount;
        }

        private void Resolve()
        {
            if (_incomingDamage > _availableBlock)
            {
                var damage = _incomingDamage - _availableBlock;
                State.HP -= damage;
                Event.Publish(new PlayerDamaged { Amount = damage, Target = State.Player });
            }
            else
            {
                _onNotDamaged.ForEach(Event.Publish);
            }
            _incomingDamage = 0;
            _availableBlock = 0;
            _onNotDamaged = new List<object>();
        }
    }
}
