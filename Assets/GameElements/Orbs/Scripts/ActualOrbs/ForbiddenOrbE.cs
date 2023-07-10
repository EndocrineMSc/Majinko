using ManaManagement;
using PeggleWars.ScrollDisplay;
using System.Collections;
using UnityEngine;

namespace Orbs
{
    internal class ForbiddenOrbE : ForbiddenOrb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, the power of this dark spell orb will destroy 1 mana in each vial. " +
                "However, you sense that there is more to its power...";
        }

        internal override IEnumerator OrbEffect()
        {
            SpriteRenderer spriteRenderer = _forbiddenE.GetComponent<SpriteRenderer>();

            if (!spriteRenderer.enabled)
            {
                spriteRenderer.enabled = true;
            }
            ManaPool manaPool = ManaPool.Instance;
            manaPool.SpendMana(1, 1, 1);
            yield return null;
        }
    }
}
