using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/ForbiddenXOrbData")]
    public class ForbiddenOrbXData : OrbData
    {
        public override void OrbEffect()
        {
            var forbiddenX = GameObject.FindGameObjectWithTag("ForbiddenX");
            forbiddenX.GetComponent<SpriteRenderer>().enabled = true;
            ExodiaWinCondtionTracker.SetForbiddenXActive();

            if (ExodiaWinCondtionTracker.CheckForExodiaWin())
                UtilityEvents.RaiseLevelVictory();

            if (OrbManager.Instance != null)
                OrbManager.Instance.SetRandomOrbInactive();
            else
                Debug.Log("Forbidden X Orb thinks OrbManager is null!");
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Forbidden Orb X</b><size=20%>\n\n<size=100%>When hit, " +
                "the power of this dark spell orb will inactivate a random orb in the <b>Arena</b>. " +
                "However, you sense that there is more to its power...";
        }
    }
}
