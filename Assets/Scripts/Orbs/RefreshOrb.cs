using System.Collections;

namespace PeggleWars.Orbs.RefreshOrb
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

        protected override void AdditionalEffectsOnCollision()
        {
            StartCoroutine(OrbEffect());
            OrbManager.Instance.CheckForRefreshOrbs();
        }

        #endregion
    }
}
