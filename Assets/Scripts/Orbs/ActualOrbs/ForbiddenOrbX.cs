using PeggleWars.ManaManagement;
using PeggleWars.ScrollDisplay;
using PeggleWars.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleWars.Orbs
{
    internal class ForbiddenOrbX : ForbiddenOrb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, the power of this dark spell orb will inactive a random orb in the \"Arena\". " +
                "However, you sense that there is more to its power...";
        }

        internal override IEnumerator OrbEffect()
        {
            SpriteRenderer spriteRenderer = _forbiddenX.GetComponent<SpriteRenderer>();

            if(!spriteRenderer.enabled)
            {
                spriteRenderer.enabled = true;
            }
            //ToDo: set random orb inactive
            yield return null;
        }
    }
}
