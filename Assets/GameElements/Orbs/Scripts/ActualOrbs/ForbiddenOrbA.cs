using Characters.Enemies;
using Utility;
using System.Collections;
using UnityEngine;

namespace Orbs
{
    internal class ForbiddenOrbA : ForbiddenOrb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Forbidden Sphere A</b><size=20%>\n\n<size=100%>Upon being hit, " +
                "the power of this dark spell sphere will apply a stack of <b>Rage</b> on the closest enemy. " +
                "However, you sense that there is more to its power...";
        }

        internal override IEnumerator OrbEffect()
        {
            SpriteRenderer spriteRenderer = _forbiddenA.GetComponent<SpriteRenderer>();

            if(!spriteRenderer.enabled)
                spriteRenderer.enabled = true;

            if (EnemyManager.Instance.EnemiesInScene.Count > 0)
                EnemyManager.Instance.EnemiesInScene[0].ApplyEnraged();

            yield return null;
        }
    }
}
