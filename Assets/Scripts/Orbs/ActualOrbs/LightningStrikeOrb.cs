using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.PlayerAttacks;

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

        #endregion
    }
}
