using System;
using System.Collections.Generic;
using System.Linq;
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
            Event.Subscribe<StatusApplied>(OnStatusApplied, this);
            Event.Subscribe<StatusRemoved>(OnStatusRemoved, this);
            Event.Subscribe<EnergyGained>(OnEnergyGained, this);
            Event.Subscribe<EnergyLossed>(OnEnergyLossed, this);
            Event.Subscribe<CardResolutionBegun>(e => Resolve(), this);
            Event.Subscribe<PlayerDamaged>(OnPlayereDamaged, this);
        }

        private void OnAttacked(PlayerDamageProposed e) => ExecuteIfTarget(e.Target, () => State.IncomingDamage += e.Amount);

        private void OnDefended(PlayerBlockProposed e) => ExecuteIfTarget(e.Target, () => State.AvailableBlock += e.Amount);

        private void OnDamagedEffectQueued(DamageEffectQueued e) => ExecuteIfTarget(e.Target, () => State.OnDamaged.Add(e.Event));

        private void OnNotDamagedEffectQueued(NotDamagedEffectQueued e) => ExecuteIfTarget(e.Target, () => State.OnNotDamaged.Add(e.Event));

        private void OnDamageBlockedEffectQueued(DamageBlockedEffectQueued e) => ExecuteIfTarget(e.Target, () => State.OnDamageBlocked.Add(e.Event));

        private void OnDamageNotBlockedEffectQueued(DamageNotBlockedEffectQueued e) => 
            ExecuteIfTarget(e.Target, () => State.OnDamageNotBlocked.Add(e.Event));

        private void OnCardTypeSelected(CardSelected e)
        {
            if (OpponentState.NextAttackBonus > 0 && e.Player != _player && _data.AllCards[e.CardId].Type == CardType.Attack)
            {
                State.IncomingDamage += OpponentState.NextAttackBonus;
                OpponentState.NextAttackBonus = 0;
            }
        }

        private void OnNextAttackEmpowered(NextAttackEmpowered e) => ExecuteIfTarget(e.Target, () => State.NextAttackBonus += e.Amount);

        private void OnDamageTakenMultiplied(DamageTakenMultiplied e) => 
            ExecuteIfTarget(e.Target, () => State.DamageTakenMultipliers.Add(e.Multiplier));

        private void OnBlockRecievedMultiplied(BlockRecievedMultiplied e) => 
            ExecuteIfTarget(e.Target, () => State.BlockRecievedMultiplier.Add(e.Multiplier));

        private void OnStatusApplied(StatusApplied e) => ExecuteIfTarget(e.Target, () => State.Statuses.Add(e.Status));

        private void OnStatusRemoved(StatusRemoved e) =>
            ExecuteIfTarget(e.Target, () =>
            {
                if (State.Statuses.Any(x => x.Name == e.Name))
                    State.Statuses.Remove(State.Statuses.First(x => x.Name == e.Name));
            });

        private void OnEnergyGained(EnergyGained e) => ExecuteIfTarget(e.Target, () => State.Energy += e.Amount);

        private void OnEnergyLossed(EnergyLossed e) => ExecuteIfTarget(e.Target, () => State.Energy -= e.Amount);

        private void ExecuteIfTarget(Player target, Action action)
        {
            if (target == _player)
                action();
        }

        private void Resolve()
        {
            decimal incomingDamageDec = State.IncomingDamage;
            State.DamageTakenMultipliers.ForEach(x => incomingDamageDec = x * incomingDamageDec);
            var incomingDamage = (int)Math.Floor(incomingDamageDec);
            decimal availableBlockDec = State.AvailableBlock;
            State.BlockRecievedMultiplier.ForEach(x => availableBlockDec = availableBlockDec * x);
            var availableBlock = (int) Math.Floor(availableBlockDec);
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

        private void Reset()
        {
            State.IncomingDamage = 0;
            State.AvailableBlock = 0;
            State.OnDamaged = new List<object>();
            State.OnNotDamaged = new List<object>();
            State.OnDamageBlocked = new List<object>();
            State.OnDamageNotBlocked = new List<object>();
            State.DamageTakenMultipliers = new List<decimal>();
            State.BlockRecievedMultiplier = new List<decimal>();
        }

        private void OnPlayereDamaged(PlayerDamaged e)
        {
            if (e.Target == _player)
                State.HP -= e.Amount;
        }
    }
}
