using PeggleWars.Cards;
using System.Collections;

namespace PeggleWars.Orbs
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
    }
}
