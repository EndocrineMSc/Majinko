using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleOrbs.OrbActions;
using EnumCollection;
using PeggleWars.Audio;

namespace PeggleOrbs.RefreshOrb
{
    public class RefreshOrb : Orb
    {
        #region Public Functions

        public override IEnumerator OrbEffect()
        {
            StartCoroutine(base.OrbEffect());
            OrbManager.Instance.SetAllOrbsActive();
            yield return null;
        }

        #endregion

        #region Protected Functions

        protected override void AdditionalEffects()
        {
            StartCoroutine(OrbEffect());
            OrbManager.Instance.CheckForRefreshOrbs();
        }

        #endregion
    }
}
