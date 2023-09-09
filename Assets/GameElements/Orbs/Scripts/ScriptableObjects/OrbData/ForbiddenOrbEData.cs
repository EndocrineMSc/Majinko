using UnityEngine;
using Utility;
using ManaManagement;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/ForbiddenEOrbData")]
    public class ForbiddenOrbEData : OrbData
    {
        public override void OrbEffect()
        {
            var forbiddenE = GameObject.FindGameObjectWithTag("ForbiddenE");
            forbiddenE.GetComponent<SpriteRenderer>().enabled = true;
            ExodiaWinCondtionTracker.SetForbiddenEActive();

            if (ExodiaWinCondtionTracker.CheckForExodiaWin())
                UtilityEvents.RaiseLevelVictory();

            if (ManaPool.Instance != null)
                ManaPool.Instance.SpendMana(1, 1, 1);
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Forbidden Orb E</b><size=20%>\n\n<size=100%>When hit, " +
                "the power of this dark spell orb will destroy <b>1 Mana</b> in each vial. " +
                "However, you sense that there is more to its power...";
        }
    }
}
