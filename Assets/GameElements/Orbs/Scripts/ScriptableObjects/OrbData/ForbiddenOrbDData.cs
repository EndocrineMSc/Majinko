using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/ForbiddenDOrbData")]
    public class ForbiddenOrbDData : OrbData
    {
        public override void OrbEffect()
        {
            var forbiddenD = GameObject.FindGameObjectWithTag("ForbiddenD");
            forbiddenD.GetComponent<SpriteRenderer>().enabled = true;
            ExodiaWinCondtionTracker.SetForbiddenDActive();

            if (ExodiaWinCondtionTracker.CheckForExodiaWin())
                UtilityEvents.RaiseLevelVictory();

            ObscuringImageHandler.RaiseObscuringImageFade();
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Forbidden Orb D</b><size=20%>\n\n<size=100%>When hit, " +
                "the power of this dark spell orb will obscure the <b>Arena</b> for the next turn. " +
                "However, you sense that there is more to its power...";
        }
    }
}
