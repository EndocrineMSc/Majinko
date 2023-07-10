using Characters.Enemies;
using PeggleWars.ScrollDisplay;
using System.Collections;
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
                spriteRenderer.enabled = true;

            if (EnemyManager.Instance.EnemiesInScene.Count > 0)
                EnemyManager.Instance.EnemiesInScene[0].ApplyEnraged();

            yield return null;
        }
    }
}
