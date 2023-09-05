using Utility;
using System.Collections;
using UnityEngine;
using Spheres;

namespace Orbs
{
    public class StoneOrb : Orb
    {
        [SerializeField] private Sprite _crumblingOrb;
        [SerializeField] private ParticleSystem _crumbleSystem;
        private SpriteRenderer _renderer;

        protected override void SetReferences()
        {
            base.SetReferences();
            _renderer = GetComponent<SpriteRenderer>();
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Stone Orb</b><size=20%>\n\n<size=100%>This orb has <b>Stalwart-1</b>. It will endure an additional hit before popping.";
        }

        public override IEnumerator OrbEffect()
        {
            //not needed
            yield return null;
        }

        protected override void AdditionalEffectsOnCollision()
        {
            //not needed
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<IAmSphere>(out _) && _stalwartHits > 0)
            {
                _renderer.sprite = _crumblingOrb;
                _crumbleSystem.Play();
            }

            base.OnCollisionEnter2D(collision);
        }
    }
}
