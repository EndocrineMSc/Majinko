using Cards;
using Utility;
using System.Collections;
using Characters;

namespace Orbs
{
    internal class FastHandsOrb : Orb
    {
        internal override IEnumerator OrbEffect()
        {
            PlayerConditionTracker.AddFastHandsStacks();
            yield return null;
        }

        protected override void AdditionalEffectsOnCollision()
        {
            StartCoroutine(OrbEffect());
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Fast Hands Orb</b><size=20%>\n\n<size=100%>Upon being hit, this orb grants one stack of <b>Fast Hands</b>. " +
                "Draw an extra card next turn for each stack of <b>Fast Hands</b> you have.";
        }
    }
}
