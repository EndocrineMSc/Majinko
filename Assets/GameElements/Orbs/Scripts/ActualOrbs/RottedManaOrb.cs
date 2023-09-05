using Utility;
using System.Collections;

namespace Orbs
{
    public class RottedManaOrb : Orb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Rotten Mana Orbs</b><size=20%>\n\n<size=100%>Upon being hit, this orb spawns " +
                "<b>1 Rotten Mana</b>. \nWhen consumed to play cards, player damage is lowered that turn depending on the amount <b>Rotten Mana</b> consumed.";
        }

        protected override void AdditionalEffectsOnCollision()
        {
            //not needed
        }

        public override IEnumerator OrbEffect()
        {
            //not needed
            yield return null;
        }
    }
}
