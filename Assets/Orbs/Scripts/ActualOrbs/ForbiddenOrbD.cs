using PeggleWars.ScrollDisplay;
using System.Collections;
using UnityEngine;

namespace Orbs
{
    internal class ForbiddenOrbD : ForbiddenOrb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, the power of this dark spell orb will obscure the \"Arena\" for the next turn. " +
                "However, you sense that there is more to its power...";
        }

        internal override IEnumerator OrbEffect()
        {
            SpriteRenderer spriteRenderer = _forbiddenD.GetComponent<SpriteRenderer>();

            if(!spriteRenderer.enabled)
            {
                spriteRenderer.enabled = true;
            }
            //ToDo: obscure board next turn
            yield return null;
        }
    }
}