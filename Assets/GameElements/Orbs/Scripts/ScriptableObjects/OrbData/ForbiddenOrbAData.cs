using UnityEngine;
using Utility;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/ForbiddenAOrbData")]
    public class ForbiddenOrbAData : OrbData
    {
        public override void OrbEffect()
        {
            var forbiddenA = GameObject.FindGameObjectWithTag("ForbiddenA");
            forbiddenA.GetComponent<SpriteRenderer>().enabled = true;
            ExodiaWinCondtionTracker.SetForbiddenAActive();
            if (ExodiaWinCondtionTracker.CheckForExodiaWin())
                UtilityEvents.RaiseLevelVictory();

            //ToDo: Apply Enraged to enemy - do I already have a tooltip for that? Functionality should stand already.
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Forbidden Orb A</b><size=20%>\n\n<size=100%>When hit, " +
                "the power of this dark spell orb will apply a stack of <b>Rage</b> on the closest enemy. " +
                "However, you sense that there is more to its power...";
        }
    }
}
