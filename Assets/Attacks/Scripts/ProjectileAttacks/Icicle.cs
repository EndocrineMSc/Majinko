using Audio;
using Characters.Enemies;
using UnityEngine;

namespace Attacks
{
    internal class Icicle : ProjectileAttack
    {
        [SerializeField] protected int _freezingStacks = 5;
        [SerializeField] protected int _frozenThreshold = 20;

        public override string Bark { get; } = "Icicle!";

        //Do special stuff in here
        private void Awake()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
        }

        protected override void OnHitPolish()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }

        protected override void AdditionalEffectsOnImpact()
        {
            Enemy enemy = _collider.GetComponent<Enemy>();
            enemy.ApplyFreezing(_freezingStacks);

            int randomChance = UnityEngine.Random.Range(0, 101);
            if (randomChance < _frozenThreshold)
            {
                enemy.ApplyFrozen();
            }
        }
    }
}
