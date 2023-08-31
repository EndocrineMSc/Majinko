using System.Collections;
using UnityEngine;
using Attacks;
using Utility;
using Unity.VisualScripting;

namespace Orbs
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
            _hailStorm.ShootAttack(aoeTargetObject.transform.position + new Vector3(0, 1, 0));
            yield return null;
            Destroy(gameObject);
        }

        //will be called OnCollisionEnter2D from parent
        protected override void AdditionalEffectsOnCollision()
        {
            OrbActionManager.Instance.AddOrbToActionList(this);            
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Hail Storm Orb</b><size=20%>\n\n<size=100%>Will cast a <b>Hail Storm</b>, " +
                "hitting all enemies for <b>10 damage</b> and applying <b>10 Freezing</b> on them. Has a moderate change to also apply <b>Frozen</b> to each enemy.";
        }

        #endregion
    }
}
