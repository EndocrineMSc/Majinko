using Characters;
using Characters.Enemies;
using Utility;
using System.Collections;
using UnityEngine;
using Spheres;

namespace Orbs
{
    public class IntangibleEnemyOrb : Orb
    {
        public override void SetDisplayDescription()
        {
            ScrollDisplayer scrollDisplayer = GetComponent<ScrollDisplayer>();
            scrollDisplayer.DisplayDescription = "<size=120%><b>Intangible Orb</b><size=20%>\n\n<size=100%>Will turn all enemies with the " +
                "ability to do so <b>Intangible</b> upon being hit. This prevents them from being hit by most attacks.";
        }

        protected override void AdditionalEffectsOnCollision()
        {
            StartCoroutine(OrbEffect());
        }

        public override IEnumerator OrbEffect()
        {
            EnemyEvents.RaiseIntangibleTriggered();
            yield return null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<IAmSphere>(out _))
            {
                GetComponent<Collider2D>().enabled = false;
                PlayOrbOnHitSound();
                OnCollisionVisualPolish();
                SpawnMana();
                ReplaceHitOrb();
                AdditionalEffectsOnCollision();
                StartCoroutine(DestroyOrbWithDelay());
            }
        }
    }
}