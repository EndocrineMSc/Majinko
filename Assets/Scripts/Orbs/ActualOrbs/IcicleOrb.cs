using System.Collections;
using UnityEngine;
using PeggleWars.Attacks;
using PeggleWars.ScrollDisplay;

namespace PeggleWars.Orbs
{
    internal class IcicleOrb : Orb
    {
        #region Fields and Properties

        [SerializeField] private Attack _icicle;

        #endregion

        #region Functions

        protected override void SetReferences()
        {
            base.SetReferences();
            _icicle.SetAttackInstantiatePosition(Player.Instance.transform);
        }

        internal override IEnumerator OrbEffect()
        {
            _icicle.ShootAttack();
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
            displayOnScroll.DisplayDescription = "Hitting this orb will cast an icicle at the first enemy, with a chance of applying frozen.";
        }

        #endregion
    }
}
