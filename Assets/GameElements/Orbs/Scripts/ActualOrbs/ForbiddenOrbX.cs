using DG.Tweening;
using Utility;
using System.Collections;
using UnityEngine;

namespace Orbs
{
    internal class ForbiddenOrbX : ForbiddenOrb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Forbidden Sphere X</b><size=20%>\n\n<size=100%>Upon being hit, " +
                "the power of this dark spell sphere will inactivate a random sphere in the <b>Arena</b>. " +
                "However, you sense that there is more to its power...";
        }

        internal override IEnumerator OrbEffect()
        {
            SpriteRenderer spriteRenderer = _forbiddenX.GetComponent<SpriteRenderer>();

            if(!spriteRenderer.enabled)
                spriteRenderer.enabled = true;

            SetRandomOrbInactive();

            yield return null;
        }

        private void SetRandomOrbInactive()
        {
            bool orbIsSetInactive = false;
            int safetyStop = 0;

            while(!orbIsSetInactive)
            {
                int orbIndex = UnityEngine.Random.Range(0, OrbManager.Instance.SceneOrbList.Count);
                Orb orb = OrbManager.Instance.SceneOrbList[orbIndex];
                
                if (orb != null && orb.OrbIsActive)
                {
                    StartCoroutine(TweenThenSetOrbInactive(orb));
                    orbIsSetInactive = true;
                }

                safetyStop++;
                if (safetyStop > 50)
                    orbIsSetInactive = true;
            }
        }

        private IEnumerator TweenThenSetOrbInactive(Orb orb)
        {
            orb.transform.DOPunchScale(new(6, 6, 6), 0.2f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(0.2f);
            orb.SetOrbInactive();
        }
    }
}
