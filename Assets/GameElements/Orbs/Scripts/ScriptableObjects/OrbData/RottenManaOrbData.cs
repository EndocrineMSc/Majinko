using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/RottenManaOrbData")]
    public class RottenManaOrbData : OrbData
    {
        public override void OrbEffect()
        {
            //none needed, mana spawning is sole effect
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Rotten Mana Orbs</b><size=20%>\n\n<size=100%>When hit, will" +
                " spawn <b>" + (_amountManaSpawned / 10) + "Rotten Mana</b>. \nWhen consumed to play cards, " +
                "will apply <b>Sickness</b> to the player depending on the amount of mana consumed," +
                "lowering the attack damage that turn for each stack applied.";
        }
    }
}
