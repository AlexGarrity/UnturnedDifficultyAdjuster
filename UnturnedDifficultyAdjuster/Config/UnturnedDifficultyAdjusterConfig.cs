using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UnturnedDifficultyAdjuster.Config
{
    public static class UnturnedDifficultyAdjusterConfig
    {
        public static GeneralWeaponOverride RangedWeaponOverride;
        public static GeneralWeaponOverride MeleeWeaponOverride;
        public static LimbDamageOverrides LimbDamageModifiers;

        public static readonly Dictionary<ushort, SpecificWeaponOverride> WeaponOverrides =
            new Dictionary<ushort, SpecificWeaponOverride>();

        public static void ParseConfig(IConfiguration configuration, ILogger logger)
        {
            logger.LogDebug("Parsing config");
            
            var meleeSection = configuration.GetSection("weapons:global:melee");
            MeleeWeaponOverride = new GeneralWeaponOverride
            {
                CanStun = meleeSection.GetSection("can_stun").Get<bool>(),
                DamageModifier = meleeSection.GetSection("damage_modifier").Get<float>(),
                CritChance = meleeSection.GetSection("crit_chance").Get<float>(),
                StunOnCrit = meleeSection.GetSection("stun_on_crit").Get<bool>()
            };
            logger.LogDebug($"The melee weapon config is as follows:\n{MeleeWeaponOverride}");

            var rangedSection = configuration.GetSection("weapons:global:ranged");
            RangedWeaponOverride = new GeneralWeaponOverride
            {
                CanStun = rangedSection.GetSection("can_stun").Get<bool>(),
                DamageModifier = rangedSection.GetSection("damage_modifier").Get<float>(),
                CritChance = rangedSection.GetSection("crit_chance").Get<float>(),
                StunOnCrit = rangedSection.GetSection("stun_on_crit").Get<bool>()
            };
            logger.LogDebug($"The ranged weapon config is as follows:\n{RangedWeaponOverride}");

            var overridesSection = configuration.GetSection("weapons:overrides");
            foreach (var overrideSection in overridesSection.GetChildren())
            {
                if (!ushort.TryParse(overrideSection.Key, out var id))
                {
                    logger.LogWarning(
                        $"Couldn't create a config for override with key '{overrideSection.Key}'. This should be a number representing the item ID.");
                    continue;
                }
                var weaponOverride = new SpecificWeaponOverride
                {
                    CanStun = overrideSection.GetSection("can_stun").Get<bool>(),
                    Damage = overrideSection.GetSection("damage").Get<ushort>(),
                    CritDamage = overrideSection.GetSection("crit_damage").Get<ushort>(),
                    CritChance = overrideSection.GetSection("crit_chance").Get<float>(),
                    StunOnCrit = overrideSection.GetSection("stun_on_crit").Get<bool>()
                };

                logger.LogDebug($"Generated the following config for Item with ID {id}:\n{weaponOverride}");
                WeaponOverrides.Add(id, weaponOverride);
            }

            var limbDamageSection = configuration.GetSection("weapons:global:limb_damage_modifiers");
            LimbDamageModifiers = new LimbDamageOverrides
            {
                Body = limbDamageSection.GetSection("body").Get<float>(),
                Arm = limbDamageSection.GetSection("arms").Get<float>(),
                Leg = limbDamageSection.GetSection("legs").Get<float>(),
                Head = limbDamageSection.GetSection("head").Get<float>(),
                Spine = limbDamageSection.GetSection("spine").Get<float>()
            };
            logger.LogDebug($"The limb damage config is as follows:\n{LimbDamageModifiers}");
        }
    }
}