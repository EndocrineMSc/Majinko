using EnumCollection;
using PeggleWars.Audio;
using PeggleWars.Characters;
using PeggleWars.Orbs;
using PeggleWars.ScrollDisplay;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleWars.Enemies
{
    internal class WraithCaster : RangedEnemy, ICanBeIntangible
    {
        public int IntangibleStacks { get; private set; } = 0;
        private int _amountIntangibleOrbs = 0;

        #region Functions

        protected override void OnEndEnemyTurn()
        {
            base.OnEndEnemyTurn();
            OrbManager.Instance.SwitchOrbs(OrbType.IntangibleEnemyOrb);
            _amountIntangibleOrbs += 1;
            HandleIntangibleStacks();
        }

        protected override void OnDeathEffect()
        {            
            int orbsToBeSwitched = _amountIntangibleOrbs;

            foreach (Orb orb  in OrbManager.Instance.SceneOrbList)
            {
                if (orbsToBeSwitched > 0)
                {
                    OrbManager.Instance.ReplaceOrbOfType(OrbType.IntangibleEnemyOrb);
                    orbsToBeSwitched--;
                }
            }            
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "A minor wraith. Most likely created out of one of your former apprentice compatriots." +
                "Will attack every other turn. Spawns intangible orbs in the arena granting them intagible when the orbs are hit.";
        }


        #region Intangible

        public void SetIntangible(int intangibleStacks = 1)
        {
            IntangibleStacks += intangibleStacks;

            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            Color alpha = spriteRenderer.color;
            alpha.a = 0.5f;
            spriteRenderer.color = alpha;

            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
        }

        public void RemoveIntangible()
        {
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            Color alpha = spriteRenderer.color;
            alpha.a = 1f;
            spriteRenderer.color = alpha;

            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = true;
        }

        public void HandleIntangibleStacks()
        {
            if (IntangibleStacks > 0)
            {
                IntangibleStacks--;
            }
            if (IntangibleStacks <= 0)
            {
                IntangibleStacks = 0;
                RemoveIntangible();
            }
        }

        #endregion

        #region Animation

        protected override void TriggerSpawnAnimation()
        {
            //ToDo;
        }

        protected override void TriggerHurtAnimation()
        {
            //ToDo;
        }

        protected override void TriggerDeathAnimation()
        {
            //ToDo;
        }
        protected override void StartMovementAnimation()
        {
            //ToDo
        }

        protected override void StopMovementAnimation()
        {
            //ToDo
        }

        protected override void TriggerAttackAnimation()
        {
            //ToDo
        }

        #endregion

        #region Sounds

        protected override void PlaySpawnSound()
        {
            //AudioManager.Instance.PlaySoundEffectOnce(SFX._0009_ZombieSpawn);
        }

        protected override void PlayDeathSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0011_ZombieDeath);
        }


        protected override void PlayHurtSound()
        {
            //ToDo
        }

        #endregion

        #endregion
    }
}
