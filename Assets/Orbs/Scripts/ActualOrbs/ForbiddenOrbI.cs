using PeggleWars.ScrollDisplay;
using System.Collections;
using UnityEngine;

namespace Orbs
{
    internal class ForbiddenOrbI : ForbiddenOrb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, the power of this dark spell orb will inflict a point of \"Illness\", reducing attack damage temporarily. " +
                "However, you sense that there is more to its power...";
        }

        internal override IEnumerator OrbEffect()
        {
            SpriteRenderer spriteRenderer = _forbiddenI.GetComponent<SpriteRenderer>();

            if(!spriteRenderer.enabled)
            {
                spriteRenderer.enabled = true;
            }
            //ToDo: reduce PlayerDamage for a turn;
            yield return null;
        }
    }
}
