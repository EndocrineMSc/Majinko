using Characters;
using Utility;
using System.Collections;
using UnityEngine;

namespace Orbs
{
    internal class ForbiddenOrbO : ForbiddenOrb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, the power of this dark spell orb will deal 1 damage to the player. " +
                "However, you sense that there is more to its power...";
        }

        internal override IEnumerator OrbEffect()
        {
            SpriteRenderer spriteRenderer = _forbiddenO.GetComponent<SpriteRenderer>();

            if(!spriteRenderer.enabled)
            {
                spriteRenderer.enabled = true;
            }

            Player.Instance.TakeDamage(1);
            yield return null;
        }
    }
}
