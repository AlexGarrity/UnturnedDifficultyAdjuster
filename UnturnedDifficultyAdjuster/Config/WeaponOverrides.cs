using SDG.Unturned;

namespace UnturnedDifficultyAdjuster.Config
{
    public abstract class WeaponOverride
    {
        public bool CanStun;
        public float CritChance;
        public bool StunOnCrit;

        protected WeaponOverride(bool canStun = false, bool stunOnCrit = true, float critChance = 0.1F)
        {
            CanStun = canStun;
            StunOnCrit = stunOnCrit;
            CritChance = critChance;
        }

        protected float GetLimbDamageModifier(LimbDamageOverrides damageModifiers, ELimb limb)
        {
            var limbDamageModifier = 1.0F;
            switch (limb)
            {
                case ELimb.SKULL:
                    limbDamageModifier = damageModifiers.Head;
                    break;
                case ELimb.LEFT_LEG:
                case ELimb.LEFT_FOOT:
                case ELimb.RIGHT_LEG:
                case ELimb.RIGHT_FOOT:
                    limbDamageModifier = damageModifiers.Leg;
                    break;
                case ELimb.LEFT_ARM:
                case ELimb.LEFT_HAND:
                case ELimb.RIGHT_ARM:
                case ELimb.RIGHT_HAND:
                    limbDamageModifier = damageModifiers.Arm;
                    break;
                case ELimb.SPINE:
                    limbDamageModifier = damageModifiers.Spine;
                    break;
                default:
                    limbDamageModifier = damageModifiers.Body;
                    break;
            }

            return limbDamageModifier;
        }

        public abstract ushort CalculateDamage(ushort originalDamage, ELimb limb);
    }

    public class GeneralWeaponOverride : WeaponOverride
    {
        public static readonly GeneralWeaponOverride Default = new GeneralWeaponOverride();
        public float DamageModifier;

        public GeneralWeaponOverride(bool canStun = false, bool stunOnCrit = true, float critChance = 0.1F,
            float damageModifier = 1.0F) : base(canStun, stunOnCrit, critChance)
        {
            DamageModifier = damageModifier;
        }

        public override ushort CalculateDamage(ushort originalDamage, ELimb limb)
        {
            var damageBeforeLimbsFactoredIn = originalDamage / GetLimbDamageModifier(LimbDamageOverrides.Default, limb);
            var damageWithNewLimbModifiers = damageBeforeLimbsFactoredIn *
                                             GetLimbDamageModifier(UnturnedDifficultyAdjusterConfig.LimbDamageModifiers,
                                                 limb);
            return (ushort)(damageWithNewLimbModifiers * DamageModifier);
        }

        public override string ToString()
        {
            return
                $"Can Stun: {CanStun}\tWill Stun on Crit: {StunOnCrit}\tDamage Modifier: {DamageModifier}%\tCrit Chance: {CritChance * 100.0F}%";
        }
    }

    public class SpecificWeaponOverride : WeaponOverride
    {
        public static readonly SpecificWeaponOverride Default = new SpecificWeaponOverride();
        public ushort CritDamage;
        public ushort Damage;

        public SpecificWeaponOverride(bool canStun = false, bool stunOnCrit = true,
            float critChance = 0.1F, ushort damage = 1) : base(canStun, stunOnCrit, critChance)
        {
            Damage = damage;
        }

        public override ushort CalculateDamage(ushort originalDamage, ELimb limb)
        {
            return (ushort)(Damage * GetLimbDamageModifier(LimbDamageOverrides.Default, limb));
        }

        public override string ToString()
        {
            return
                $"Can Stun: {CanStun}\tWill Stun on Crit: {StunOnCrit}\tDamage: {Damage}\tCrit Damage: {CritDamage}\tCrit Chance: {CritChance * 100.0F}%";
        }
    }
}