using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenMod.API.Eventing;
using OpenMod.Core.Eventing;
using OpenMod.Unturned.Zombies.Events;
using SDG.Unturned;
using UnturnedDifficultyAdjuster.Config;

namespace UnturnedDifficultyAdjuster.Events
{
    public class UnturnedZombieDamagingEventListener : IEventListener<UnturnedZombieDamagingEvent>
    {
        private readonly ILogger<UnturnedZombieDamagingEventListener> m_Logger;

        public UnturnedZombieDamagingEventListener(ILogger<UnturnedZombieDamagingEventListener> logger)
        {
            m_Logger = logger;
        }

        [EventListener(Priority = EventListenerPriority.Highest)]
        public Task HandleEventAsync(object sender, UnturnedZombieDamagingEvent @event)
        {
            if (@event.Instigator == null)
                // A player didn't damage this zombie, do nothing
                return Task.CompletedTask;

            // Check if we have a specific override
            var itemID = @event.Instigator.Player.equipment.itemID;
            WeaponOverride weaponOverride = null;

            if (UnturnedDifficultyAdjusterConfig.WeaponOverrides.TryGetValue(itemID, out var @override))
                weaponOverride = @override;
            else
                weaponOverride = @event.Instigator.Player.equipment.asset.type == EItemType.GUN
                    ? UnturnedDifficultyAdjusterConfig.RangedWeaponOverride
                    : UnturnedDifficultyAdjusterConfig.MeleeWeaponOverride;

            // Disable base game stun mechanic if requested
            if (weaponOverride.CanStun)
                @event.StunOverride = EZombieStunOverride.Never;

            // Check if we should crit
            var random = new Random();
            if (random.NextDouble() < weaponOverride.CritChance)
            {
                if (weaponOverride.StunOnCrit)
                    @event.StunOverride = EZombieStunOverride.Always;
            }
            else
            {
                @event.DamageAmount = weaponOverride.CalculateDamage(@event.DamageAmount, @event.Limb);
            }

            return Task.CompletedTask;
        }
    }
}