using System;
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
            Event.Subscribe<DamageEffectQueued>(OnDamagedEffectQueued, this);
            Event.Subscribe<NotDamagedEffectQueued>(OnNotDamagedEffectQueued, this);
            Event.Subscribe<DamageBlockedEffectQueued>(OnDamageBlockedEffectQueued, this);
            Event.Subscribe<DamageNotBlockedEffectQueued>(OnDamageNotBlockedEffectQueued, this);
            Event.Subscribe<CardSelected>(OnCardTypeSelected, this);
            Event.Subscribe<NextAttackEmpowered>(OnNextAttackEmpowered, this);
            Event.Subscribe<DamageTakenMultiplied>(OnDamageTakenMultiplied, this);
            Event.Subscribe<BlockRecievedMultiplied>(OnBlockRecievedMultiplied, this);
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

        private void OnDamagedEffectQueued(DamageEffectQueued e)
        {
            if (e.Target == _player)
                State.OnDamaged.Add(e.Event);
        }

        private void OnNotDamagedEffectQueued(NotDamagedEffectQueued e)
        {
            if (e.Target == _player)
                State.OnNotDamaged.Add(e.Event);
        }

        private void OnDamageBlockedEffectQueued(DamageBlockedEffectQueued e)
        {
            if (e.Target == _player)
                State.OnDamageBlocked.Add(e.Event);
        }

        private void OnDamageNotBlockedEffectQueued(DamageNotBlockedEffectQueued e)
        {
            if (e.Target == _player)
                State.OnDamageNotBlocked.Add(e.Event);
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
            if (availableBlock > 0 && incomingDamage > 0)
            {
                Event.Publish(new DamageBlocked { Target = State.Player, Amount = Math.Min(availableBlock, incomingDamage)});
                State.OnDamageBlocked.ForEach(Event.Publish);
            }
            else 
                State.OnDamageNotBlocked.ForEach(Event.Publish);
            if (incomingDamage > availableBlock)
            {
                Event.Publish(new PlayerDamaged { Amount = incomingDamage - availableBlock, Target = State.Player });
                State.OnDamaged.ForEach(Event.Publish);
            }
            else
                State.OnNotDamaged.ForEach(Event.Publish);
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
