using Characters;
using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/FastHandsOrbData")]
    public class FastHandsOrbData : OrbData
    {
        public override void OrbEffect()
        {
            PlayerConditionTracker.AddFastHandsStacks();
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Fast Hands Orb</b><size=20%>\n\n<size=100%>Upon being hit, this orb grants " +
                "one stack of <b>Fast Hands</b>. Draw an extra card next turn for each stack of <b>Fast Hands</b> you have.";
        }
    }
}
