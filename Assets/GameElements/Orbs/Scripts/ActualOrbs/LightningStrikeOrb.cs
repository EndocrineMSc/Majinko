using System.Collections;
using UnityEngine;
using Attacks;
using Utility;
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
            var targetPosition = new Vector3(targetEnemy.position.x, 8.675f, targetEnemy.position.z);
            _lightningStrike.ShootAttack(targetPosition);
            yield return null;
            Destroy(gameObject);
        }

        protected override void AdditionalEffectsOnCollision()
        {
            OrbActionManager.Instance.AddOrbToActionList(this);
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Lightning Strike Orb</b><size=20%>\n\n<size=100%>Hitting this orb will enable " +
                "the player to cast a <b>Lightning Strike</b> on the farthest enemy, dealing <b>40 damage</b>";
        }

        #endregion
    }
}
