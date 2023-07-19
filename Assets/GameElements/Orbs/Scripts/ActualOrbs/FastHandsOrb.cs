using Cards;
using Utility;
using System.Collections;

namespace Orbs
{
    internal class FastHandsOrb : Orb
    {
        internal override IEnumerator OrbEffect()
        {
            Hand.Instance.DrawAmount += 1;
            yield return null;
        }

        protected override void AdditionalEffectsOnCollision()
        {
            StartCoroutine(OrbEffect());
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, this orb grants one stack of \"Fast Hands\". " +
                "Draw an extra card next turn for each stack of \"Fast Hands\" you have.";
        }
    }
}
