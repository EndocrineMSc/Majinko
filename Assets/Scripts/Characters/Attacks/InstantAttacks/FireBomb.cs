using EnumCollection;
using PeggleWars.Audio;
using PeggleWars.Characters.Interfaces;
using PeggleWars.Enemies;
using System.Collections;
using UnityEngine;

namespace PeggleWars.Attacks
{
    internal class FireBomb : InstantAttack, IAmAOE
    {
        [SerializeField] protected int _burningStacks = 5;

        public override string Bark { get; } = "Fire Bomb!";

        //Do special stuff in here
        private void Awake()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0003_ManaBlitz);
            HandleAOE();
        }

        protected override void OnHitPolish()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0010_BluntSpellImpact);
        }

        public void HandleAOE()
        {
            foreach (Enemy enemy in EnemyManager.Instance.EnemiesInScene)
            {
                enemy.TakeDamage(_damage);
                enemy.TakeIceDamage();
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
