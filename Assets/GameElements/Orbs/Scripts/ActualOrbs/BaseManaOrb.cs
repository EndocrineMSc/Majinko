using Utility;
using System.Collections;

namespace Orbs
{
    internal class BaseManaOrb : Orb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, this orb spawns \"Basic Mana\". The most basic form of all magic orbs.";
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
