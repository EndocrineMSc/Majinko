using EnumCollection;
using Audio;
using PeggleWars.Characters.Interfaces;
using Enemies;
using UnityEngine;

namespace PeggleWars.Attacks
{
    internal class HailStorm : InstantAttack, IAmAOE
    {
        [SerializeField] protected int _freezingStacks = 5;
        [SerializeField] protected int _frozenThreshold = 30;

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
                enemy.TakeDamage(_damage);
                enemy.TakeIceDamage();
                enemy.ApplyFreezing(_freezingStacks);

                int randomChance = UnityEngine.Random.Range(0, 101);
                if (randomChance < _frozenThreshold)
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
