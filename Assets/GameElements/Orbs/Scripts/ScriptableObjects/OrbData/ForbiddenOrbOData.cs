using UnityEngine;
using Utility;
using Characters;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/ForbiddenOOrbData")]
    public class ForbiddenOrbOData : OrbData
    {
        public override void OrbEffect()
        {
            var forbiddenO = GameObject.FindGameObjectWithTag("ForbiddenO");
            forbiddenO.GetComponent<SpriteRenderer>().enabled = true;
            ExodiaWinCondtionTracker.SetForbiddenOActive();

            if (ExodiaWinCondtionTracker.CheckForExodiaWin())
                UtilityEvents.RaiseLevelVictory();

            if (Player.Instance != null)
                Player.Instance.TakeDamage(1);
            else
                Debug.Log("Forbidden O thinks player is null!");
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Forbidden Orb O</b><size=20%>\n\n<size=100%>When hit, " +
                "the power of this dark spell orb will deal 1 damage to the player. " +
                "However, you sense that there is more to its power...";
        }
    }
}
