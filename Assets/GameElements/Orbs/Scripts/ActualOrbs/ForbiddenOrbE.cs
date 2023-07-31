using ManaManagement;
using Utility;
using System.Collections;
using UnityEngine;

namespace Orbs
{
    internal class ForbiddenOrbE : ForbiddenOrb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Forbidden Orb E</b><size=20%>\n\n<size=100%>Upon being hit, " +
                "the power of this dark spell orb will destroy <b>1 Mana</b> in each vial. " +
                "However, you sense that there is more to its power...";
        }

        internal override IEnumerator OrbEffect()
        {
            SpriteRenderer spriteRenderer = _forbiddenE.GetComponent<SpriteRenderer>();

            if (!spriteRenderer.enabled)           
                spriteRenderer.enabled = true;
            
            ManaPool.Instance.SpendMana(1, 1, 1);
            yield return null;
        }
    }
}
