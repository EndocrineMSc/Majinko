using System.Collections;
using UnityEngine;
using Attacks;
using Utility;
using Characters;

namespace Orbs
{
    internal class FireArrowOrb : Orb, IHaveBark
    {
        #region Fields and Properties

        [SerializeField] private Attack _fireArrow;
        public string Bark { get; } = "Fire Arrow!";

        #endregion

        #region Functions

        internal override IEnumerator OrbEffect()
        {
            _fireArrow.ShootAttack(Player.Instance.transform.position);
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
            displayOnScroll.DisplayDescription = "<size=120%><b>Fire Arrow Sphere</b><size=20%>\n\n<size=100%>Hitting this sphere will enable the player to cast a fire arrow on the closest enemy. " +
                "This will apply 5 stacks of <b>Burning</b> to the target.";
        }

        #endregion
    }
}
