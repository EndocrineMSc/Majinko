using System.Collections;
using UnityEngine;
using Attacks;
using Utility;
using Characters;

namespace Orbs
{
    internal class IcicleOrb : Orb
    {
        #region Fields and Properties

        [SerializeField] private Attack _icicle;

        #endregion

        #region Functions

        internal override IEnumerator OrbEffect()
        {
            _icicle.ShootAttack(Player.Instance.transform.position);
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
            displayOnScroll.DisplayDescription = "<size=120%><b>Icicle Orb</b><size=20%>\n\n<size=100%>Hitting this orb will cast an " +
                "<b>Icicle</b> at the first enemy, dealing 10 damage and applying <b>10 Freezing</b>." +
                " Has a chance to apply <b>Frozen</b>.";
        }

        #endregion
    }
}
