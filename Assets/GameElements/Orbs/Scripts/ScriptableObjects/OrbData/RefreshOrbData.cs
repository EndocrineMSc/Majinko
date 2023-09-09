using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/RefreshOrbData")]
    public class RefreshOrbData : OrbData
    {
        public override void OrbEffect()
        {
            OrbEvents.RaiseSetOrbsActive();

            if (OrbManager.Instance != null)
                OrbManager.Instance.CheckForRefreshOrbs();
            else
                Debug.Log("OrbData: Couldn't check for correct amount of refresh orbs. Hit Refresh Orb thinks" +
                    "OrbManager is null!");
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Refresh Orb</b><size=20%>\n\n<size=100%> When hit," +
                " will reactivate all orbs in the <b>Arena</b> immediately";
        }
    }
}
