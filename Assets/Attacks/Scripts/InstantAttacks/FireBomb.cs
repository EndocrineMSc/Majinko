using EnumCollection;
using Audio;
using PeggleWars.Characters.Interfaces;
using Enemies;
using System.Collections;
using UnityEngine;
using PeggleWars.Attacks;

namespace Attacks
{
    internal class FireBomb : InstantAttack, IAmAOE
    {
        [SerializeField] protected int _burningStacks = 5;

        public override string Bark { get; } = "Fire Bomb!";

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
                enemy.ApplyBurning(_burningStacks);
            }
            DestroyGameObject();
        }

        protected override void AdditionalEffectsOnImpact()
        {
            //empty
        }
    }
}
