using System.Collections;
using UnityEngine;
using PeggleWars.PlayerAttacks;

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

        #endregion
    }
}
