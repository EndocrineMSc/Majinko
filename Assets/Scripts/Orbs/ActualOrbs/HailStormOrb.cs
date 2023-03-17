using System.Collections;
using UnityEngine;
using PeggleWars.Attacks;
using PeggleWars.ScrollDisplay;

namespace PeggleWars.Orbs
{
    internal class HailStormOrb : Orb
    {
        #region Fields and Properties

        [SerializeField] private Attack _hailStorm;
        private string TARGET_PARAM = "AOE_Target";

        #endregion

        #region Functions

        protected override void SetReferences()
        {
            base.SetReferences();
            GameObject aoeTargetObject = GameObject.FindGameObjectWithTag(TARGET_PARAM);
            _hailStorm.SetAttackInstantiatePosition(aoeTargetObject.transform);
        }

        internal override IEnumerator OrbEffect()
        {
            _hailStorm.ShootAttack();
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
