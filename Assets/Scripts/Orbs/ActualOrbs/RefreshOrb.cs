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

        #endregion
    }
}
