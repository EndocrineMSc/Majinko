using Characters;
using UnityEngine;
using Utility;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/ForbiddenIOrbData")]
    public class ForbiddenOrbIData : OrbData
    {
        public override void OrbEffect()
        {
            var forbiddenI = GameObject.FindGameObjectWithTag("ForbiddenI");
            forbiddenI.GetComponent<SpriteRenderer>().enabled = true;
            ExodiaWinCondtionTracker.SetForbiddenIActive();

            if (ExodiaWinCondtionTracker.CheckForExodiaWin())
                UtilityEvents.RaiseLevelVictory();

            PlayerConditionTracker.AddSicknessStacks();
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Forbidden Orb I</b><size=20%>\n\n<size=100%>When hit, " +
                "the power of this dark spell orb will inflict a stack of <b>Illness</b> on the player" +
                ", reducing attack damage for the turn. However, you sense that there is more to its power...";
        }
    }
}
