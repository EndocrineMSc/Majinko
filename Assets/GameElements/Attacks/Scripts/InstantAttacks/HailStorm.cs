using Audio;
using Characters.Enemies;
using UnityEngine;
using PeggleWars.Attacks;

namespace Attacks
{
    internal class HailStorm : InstantAttack
    {
        public override string Bark { get; } = "Hail Storm!";

        //Do special stuff in here
        protected override void Awake()
        {
            base.Awake();
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
            HandleAOE();
        }

        protected override void OnHitPolish()
        {
            base.OnHitPolish();
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }

        public void HandleAOE()
        {
            OnHitPolish();
            foreach (Enemy enemy in EnemyManager.Instance.EnemiesInScene)
            {
                enemy.TakeDamage(_attackValues.Damage);
                enemy.ApplyFreezing(_attackValues.FreezingStacks);

                int randomChance = UnityEngine.Random.Range(0, 101);
                if (randomChance < _attackValues.PercentToFreeze)
                {
                    enemy.ApplyFrozen();
                }
            }
            Destroy(gameObject);
        }

        protected override void PlayHitSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }

        protected override void PlayAwakeSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
        }
    }
}
