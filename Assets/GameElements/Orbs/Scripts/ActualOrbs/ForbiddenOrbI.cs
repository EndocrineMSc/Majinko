using Attacks;
using Utility;
using System.Collections;
using UnityEngine;

namespace Orbs
{
    internal class ForbiddenOrbI : ForbiddenOrb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Forbidden Sphere I</b><size=20%>\n\n<size=100%>Upon being hit, " +
                "the power of this dark spell sphere will inflict a stack of <b>Illness</b>, reducing attack damage temporarily. " +
                "However, you sense that there is more to its power...";
        }

        internal override IEnumerator OrbEffect()
        {
            SpriteRenderer spriteRenderer = _forbiddenI.GetComponent<SpriteRenderer>();

            if(!spriteRenderer.enabled)
                spriteRenderer.enabled = true;

            PlayerAttackDamageManager.Instance.ModifyDamage(0.75f);

            yield return null;
        }
    }
}
