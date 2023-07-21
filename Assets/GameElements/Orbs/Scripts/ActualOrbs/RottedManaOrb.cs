using Utility;
using System.Collections;

namespace Orbs
{
    internal class RottedManaOrb : Orb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, this orb spawns \"Rotted Mana\". \nWhen consumed to play cards, player damage is lowered that turn.";
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
