using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.PlayerAttacks;
using PeggleWars.ScrollDisplay;

namespace PeggleWars.Orbs
{
    internal class LightningStrikeOrb : Orb
    {
        #region Fields and Properties

        [SerializeField] private PlayerAttack _lightningStrike;

        #endregion

        #region Functions

        internal override IEnumerator OrbEffect()
        {
            _lightningStrike.ShootAttack(_lightningStrike);
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
