using EnumCollection;
using Audio;
using UnityEngine;

namespace Attacks
{
    internal class ManaBlitz : ProjectileAttack
    {
        public override string Bark { get; } = "Mana Blitz!";

        protected override void AdditionalEffectsOnImpact(GameObject target)
        {
            //none yet
        }

        //Do special stuff in here
        protected override void Awake()
        {
            base.Awake();
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
        }

        protected override void PlayAwakeSound()
        {
            throw new System.NotImplementedException();
        }

        protected override void PlayHitSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
        }
    }
}
