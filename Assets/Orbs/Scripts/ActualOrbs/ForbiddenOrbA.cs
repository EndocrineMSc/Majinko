using PeggleWars.ManaManagement;
using PeggleWars.ScrollDisplay;
using PeggleWars.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbs
{
    internal class ForbiddenOrbA : ForbiddenOrb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, the power of this dark spell orb will enrage the closest enemy to the player, enhancing its strength. " +
                "However, you sense that there is more to its power...";
        }

        internal override IEnumerator OrbEffect()
        {
            SpriteRenderer spriteRenderer = _forbiddenA.GetComponent<SpriteRenderer>();

            if(!spriteRenderer.enabled)
            {
                spriteRenderer.enabled = true;
            }
            //ToDo: enrage first enemy
            yield return null;
        }
    }
}
