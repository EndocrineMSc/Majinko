using Utility;
using System.Collections;

namespace Orbs
{
    internal class BaseManaOrb : Orb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Base Mana Orb</b><size=20%>\n\n<size=100%>Upon being hit, this orb spawns <b>Basic Mana</b>. The most basic form of all magic orbs.";
        }

        internal override IEnumerator OrbEffect()
        {
            //not needed
            yield return null;
        }

        protected override void AdditionalEffectsOnCollision()
        {
            //not needed
        }
    }
}
