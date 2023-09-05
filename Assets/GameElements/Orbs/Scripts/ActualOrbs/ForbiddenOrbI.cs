using Attacks;
using Utility;
using System.Collections;
using UnityEngine;

namespace Orbs
{
    public class ForbiddenOrbI : ForbiddenOrb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Forbidden Orb I</b><size=20%>\n\n<size=100%>Upon being hit, " +
                "the power of this dark spell orb will inflict a stack of <b>Illness</b>, reducing attack damage temporarily. " +
                "However, you sense that there is more to its power...";
        }

        public override IEnumerator OrbEffect()
        {
            SpriteRenderer spriteRenderer = _forbiddenI.GetComponent<SpriteRenderer>();

            if(!spriteRenderer.enabled)
                spriteRenderer.enabled = true;

            PlayerAttackDamageManager.Instance.ModifyPlayerDamage(0.75f);

            yield return null;
        }
    }
}
