using EnumCollection;
using PeggleWars.Audio;
using PeggleWars.Characters.Interfaces;
using PeggleWars.Enemies;
using UnityEngine;

namespace PeggleWars.Attacks
{
    internal class FireArrow : ProjectileAttack
    {
        [SerializeField] protected int _burningStacks = 5;

        public override string Bark { get; } = "Fire Arrow!";

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
            Enemy enemy = _collision.GetComponent<Enemy>();
            enemy.TakeDamage(_damage);
            enemy.ApplyBurning(_burningStacks);
        }
    }
}
