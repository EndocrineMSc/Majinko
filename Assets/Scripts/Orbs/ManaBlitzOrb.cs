using System.Collections;
using UnityEngine;
using EnumCollection;
using PeggleWars.Orbs.OrbActions;
using PeggleWars.Audio;
using PeggleWars.PlayerAttacks;

namespace PeggleWars.Orbs.ManaBlitzOrb
{
    public class ManaBlitzOrb : Orb
    {
        #region Fields

        [SerializeField] private PlayerAttack _manaBlitz;

        #endregion

        #region Properties

        #endregion

        #region Public Functions
        public override IEnumerator OrbEffect()
        {
            StartCoroutine(base.OrbEffect());
            _manaBlitz.ShootAttack(_manaBlitz);
            yield return new WaitForSeconds(0.2f);
        }

        #endregion

        #region Protected Functions

        //will be called OnCollisionEnter2D from parent
        protected override void AdditionalEffectsOnCollision()
        {
            OrbActionManager.Instance.AddOrbToActionList(this);            
        }

        #endregion
    }
}
