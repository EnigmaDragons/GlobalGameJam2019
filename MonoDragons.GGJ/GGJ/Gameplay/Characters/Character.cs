using System.Collections.Generic;
using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Gameplay.Events;

namespace MonoDragons.GGJ.Gameplay
{
    public class Character
    {
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
            Event.Subscribe<OnDamageEffectQueued>(OnDamagedEffectQueued, this);
            Event.Subscribe<CardSelected>(OnCardTypeSelected, this);
            Event.Subscribe<NextAttackEmpowered>(OnNextAttackEmpowered, this);
            Event.Subscribe<DamageTakenMultiplied>(OnDamageTakenMultiplied, this);
            Event.Subscribe<CardResolutionBegun>(e => Resolve(), this);
            Event.Subscribe<PlayerDamaged>(OnPlayereDamaged, this);
        }

        private void OnAttacked(PlayerDamageProposed e)
        {
            if (e.Target == _player)
                State.IncomingDamage += e.Amount;
        }

        private void OnDefended(PlayerBlockProposed e)
        {
            if (e.Target == _player)
                State.AvailableBlock += e.Amount;
        }

        private void OnNotDamagedEffectQueued(OnNotDamagedEffectQueued e)
        {
            if (e.Target == _player)
                State.OnNotDamaged.Add(e.Event);
        }

        private void OnDamagedEffectQueued(OnDamageEffectQueued e)
        {
            if (e.Target == _player)
                State.OnDamaged.Add(e.Event);
        }

        private void OnCardTypeSelected(CardSelected e)
        {
            if (OpponentState.NextAttackBonus > 0 && e.Player != _player && _data.AllCards[e.CardId].Type == CardType.Attack)
            {
                State.IncomingDamage += OpponentState.NextAttackBonus;
                OpponentState.NextAttackBonus = 0;
            }
        }

        private void OnNextAttackEmpowered(NextAttackEmpowered e)
        {
            if (e.Target == _player)
                State.NextAttackBonus += e.Amount;
        }

        private void OnDamageTakenMultiplied(DamageTakenMultiplied e)
        {
            if (e.Target == _player)
                State.DamageTakenMultipliers.Add(e.Multiplier);
        }

        private void OnBlockRecievedMultiplied(BlockRecievedMultiplied e)
        {
            if (e.Target == _player)
                State.BlockRecievedMultiplier.Add(e.Multiplier);
        }

        private void Resolve()
        {
            var incomingDamage = State.IncomingDamage;
            State.DamageTakenMultipliers.ForEach(x => incomingDamage = x * incomingDamage);
            var availableBlock = State.AvailableBlock;
            State.BlockRecievedMultiplier.ForEach(x => availableBlock = availableBlock * x);
            if (incomingDamage > availableBlock)
            {
                Event.Publish(new PlayerDamaged { Amount = incomingDamage - availableBlock, Target = State.Player });
                State.OnDamaged.ForEach(Event.Publish);
            }
            else
            {
                State.OnNotDamaged.ForEach(Event.Publish);
            }
            Reset();
        }

        private void OnPlayereDamaged(PlayerDamaged e)
        {
            if (e.Target == _player)
                State.HP -= e.Amount;
        }

        private void Reset()
        {
            State.IncomingDamage = 0;
            State.AvailableBlock = 0;
            State.OnNotDamaged = new List<object>();
            State.OnDamaged = new List<object>();
            State.DamageTakenMultipliers = new List<int>();
            State.BlockRecievedMultiplier = new List<int>();
        }
    }
}
