using System.Collections;
using UnityEngine;
using PeggleWars.Attacks;
using PeggleWars.ScrollDisplay;
using PeggleWars.Utilities;

namespace PeggleWars.Orbs
{
    internal class ManaBlitzOrb : Orb, IHaveBark
    {
        #region Fields and Properties

        [SerializeField] private Attack _manaBlitz;
        public string Bark { get; } = "Mana Blitz!";

        #endregion

        #region Functions

        internal override IEnumerator OrbEffect()
        {
            _manaBlitz.ShootAttack(Player.Instance.transform.position);
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
