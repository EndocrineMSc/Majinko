using PeggleWars.ScrollDisplay;
using System.Collections;
using UnityEngine;

namespace Orbs
{
    internal class RefreshOrb : Orb
    {
        private bool _effectIsTriggered;

        #region Functions

        internal override IEnumerator OrbEffect()
        {
            yield return new WaitForSeconds(0.1f);
            OrbEvents.RaiseSetOrbsActive();
            yield return new WaitForSeconds(0.1f);
            OrbManager.Instance.CheckForRefreshOrbs();
        }

        protected override void AdditionalEffectsOnCollision()
        {
            if (!_effectIsTriggered)
            {
                _effectIsTriggered = true;
                StartCoroutine(OrbEffect());
            }
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Reactivates all orbs";
        }

        #endregion
    }
}
