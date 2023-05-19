using System.Collections;
using UnityEngine;
using PeggleWars.Attacks;
using PeggleWars.ScrollDisplay;
using PeggleAttacks.AttackVisuals.PopUps;
using Enemies;

namespace PeggleWars.Orbs
{
    internal class HailStormOrb : Orb
    {
        #region Fields and Properties

        [SerializeField] private Attack _hailStorm;
        private readonly string TARGET_PARAM = "AOE_Target";

        #endregion

        #region Functions

        internal override IEnumerator OrbEffect()
        {
            GameObject aoeTargetObject = GameObject.FindGameObjectWithTag(TARGET_PARAM);
            _hailStorm.ShootAttack(aoeTargetObject.transform.position);
            yield return new WaitForSeconds(0.2f);
        }

        //will be called OnCollisionEnter2D from parent
        protected override void AdditionalEffectsOnCollision()
        {
            OrbActionManager.Instance.AddOrbToActionList(this);            
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Will cast a hail storm, hitting all enemies and applying freezing on them. Has a moderate change to also apply frozen to each enemy.";
        }

        #endregion
    }
}
