using Audio;
using Characters.Enemies;
using UnityEngine;

namespace Attacks
{
    internal class FireArrow : ProjectileAttack
    {
        public override string Bark { get; } = "Fire Arrow!";

        //Do special stuff in here
        protected override void Awake()
        {
            base.Awake();
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
        }

        protected override void OnHitPolish()
        {
            base.OnHitPolish();
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }

        protected override void AdditionalEffectsOnImpact()
        {
            Enemy enemy = _collider.GetComponent<Enemy>();

            if (enemy != null )
                enemy.ApplyBurning(_attackValues.BurningStacks);
        }
    }
}
