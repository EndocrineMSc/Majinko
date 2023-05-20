using System.Collections;
using UnityEngine;
using Attacks;
using PeggleWars.ScrollDisplay;
using Characters.Enemies;

namespace Orbs
{
    internal class LightningStrikeOrb : Orb
    {
        #region Fields and Properties

        [SerializeField] private Attack _lightningStrike;

        #endregion

        #region Functions

        internal override IEnumerator OrbEffect()
        {
            Transform targetEnemy = EnemyManager.Instance.EnemiesInScene[EnemyManager.Instance.EnemiesInScene.Count - 1].transform;
            _lightningStrike.ShootAttack(targetEnemy.position);
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
