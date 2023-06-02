using Audio;
using Characters.Enemies;
using UnityEngine;
using PeggleWars.Attacks;

namespace Attacks
{
    internal class HailStorm : InstantAttack, IAmAOE
    {
        public override string Bark { get; } = "Hail Storm!";

        //Do special stuff in here
        private void Awake()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
            HandleAOE();
        }

        protected override void OnHitPolish()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }

        public void HandleAOE()
        {
            foreach (Enemy enemy in EnemyManager.Instance.EnemiesInScene)
            {
                enemy.TakeDamage(ModifiedDamage);
                enemy.ApplyFreezing(_attackValues.FreezingStacks);

                int randomChance = UnityEngine.Random.Range(0, 101);
                if (randomChance < _attackValues.PercentToFreeze)
                {
                    enemy.ApplyFrozen();
                }
            }
            DestroyGameObject();
        }

        protected override void AdditionalEffectsOnImpact()
        {
            //empty
        }
    }
}
