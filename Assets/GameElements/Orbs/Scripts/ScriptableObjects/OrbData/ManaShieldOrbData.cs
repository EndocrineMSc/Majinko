using UnityEngine;
using Characters;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/ManaShieldOrbData")]
    public class ManaShieldOrbData : OrbData
    {
        public override void OrbEffect()
        {
            if (Player.Instance != null)
                Player.Instance.GainShield(_shieldStacks);
            else
                Debug.Log("OrbData: Shield couldn't be applied to player. Orb thinks player is null!");
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Mana Shield Orb</b><size=20%>\n\n<size=100%>When hit, will " +
                "apply <b>" + _shieldStacks + " Shield</b> to the player immediately. Shield is lost at the " +
                "end of a turn. \n" +
                "One of the earliest spells taught to young witches.";
        }
    }
}
