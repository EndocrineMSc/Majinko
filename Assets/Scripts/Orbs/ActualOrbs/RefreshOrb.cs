using PeggleWars.ScrollDisplay;
using System.Collections;

namespace PeggleWars.Orbs
{
    internal class RefreshOrb : Orb
    {
        #region Functions

        internal override IEnumerator OrbEffect()
        {
            StartCoroutine(base.OrbEffect());
            OrbManager.Instance.SetAllOrbsActive();
            yield return null;
        }

        protected override void AdditionalEffectsOnCollision()
        {
            StartCoroutine(OrbEffect());
            OrbManager.Instance.CheckForRefreshOrbs();
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, this orb will reactivate all orbs in the arena.";
        }

        #endregion
    }
}
