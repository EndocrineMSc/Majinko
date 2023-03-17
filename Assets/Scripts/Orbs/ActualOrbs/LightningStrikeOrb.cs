using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Attacks;
using PeggleWars.ScrollDisplay;
using PeggleWars.Enemies;

namespace PeggleWars.Orbs
{
    internal class LightningStrikeOrb : Orb
    {
        #region Fields and Properties

        [SerializeField] private Attack _lightningStrike;

        #endregion

        #region Functions

        protected override void SetReferences()
        {
            base.SetReferences();
            if (EnemyManager.Instance.EnemiesInScene.Count > 0)
            {
                Transform targetEnemy = EnemyManager.Instance.EnemiesInScene[EnemyManager.Instance.EnemiesInScene.Count - 1].transform;
                _lightningStrike.SetAttackInstantiatePosition(targetEnemy);
            }

        }

        internal override IEnumerator OrbEffect()
        {
            _lightningStrike.ShootAttack();
            yield return new WaitForSeconds(0.1f);
        }

        protected override void AdditionalEffectsOnCollision()
        {
            OrbActionManager.Instance.AddOrbToActionList(this);
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Hitting this orb will enable the player to cast a \"Lightning Strike\" on the farthest enemy.";
        }

        #endregion
    }
}
