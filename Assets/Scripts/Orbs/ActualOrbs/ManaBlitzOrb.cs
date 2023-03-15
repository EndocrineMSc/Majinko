using System.Collections;
using UnityEngine;
using PeggleWars.PlayerAttacks;
using PeggleWars.ScrollDisplay;

namespace PeggleWars.Orbs
{
    internal class ManaBlitzOrb : Orb
    {
        #region Fields and Properties

        [SerializeField] private PlayerAttack _manaBlitz;

        #endregion

        #region Functions

        internal override IEnumerator OrbEffect()
        {
            StartCoroutine(base.OrbEffect());
            _manaBlitz.ShootAttack(_manaBlitz);
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
            displayOnScroll.DisplayDescription = "Hitting this orb will enable the player to cast a standard Mana Blitz on the closest enemy. " +
                "One of the earliest spells taught to wizard apprentices.";
        }

        #endregion
    }
}
