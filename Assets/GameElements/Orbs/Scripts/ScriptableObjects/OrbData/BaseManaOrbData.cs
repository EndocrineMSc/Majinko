using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/BaseManaOrbData")]
    public class BaseManaOrbData : OrbData
    {
        public override void OrbEffect()
        {
            //No additional effect
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Base Mana Orb</b><size=20%>\n\n<size=100%>Upon being hit, " +
                "this orb spawns <b>Basic Mana</b>. The most basic form of all magic orbs.";
        }
    }
}
