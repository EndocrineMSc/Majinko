using Utility;
using System.Collections;

namespace Orbs
{
    internal class RottedManaOrb : Orb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Rotten Mana Sphere</b><size=20%>\n\n<size=100%>Upon being hit, this orb spawns " +
                "<b>1 Rotten Mana</b>. \nWhen consumed to play cards, player damage is lowered that turn depending on the amount <b>Rotten Mana</b> consumed.";
        }

        protected override void AdditionalEffectsOnCollision()
        {
            //not needed
        }

        internal override IEnumerator OrbEffect()
        {
            //not needed
            yield return null;
        }
    }
}
