using Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Orbs
{
    internal class ForbiddenOrbD : ForbiddenOrb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Forbidden Sphere D</b><size=20%>\n\n<size=100%>Upon being hit, " +
                "the power of this dark spell sphere will obscure the <b>Arena</b> for the next turn. " +
                "However, you sense that there is more to its power...";
        }

        internal override IEnumerator OrbEffect()
        {
            SpriteRenderer spriteRenderer = _forbiddenD.GetComponent<SpriteRenderer>();

            if(!spriteRenderer.enabled)
                spriteRenderer.enabled = true;

            FadeImage();
            yield return null;
        }

        private void FadeImage()
        {
            ObscuringImageHandler.RaiseObscuringImageFade();
        }
    }
}
