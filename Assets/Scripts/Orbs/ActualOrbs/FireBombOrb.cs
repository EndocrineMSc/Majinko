using PeggleWars.ScrollDisplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Attacks;
using PeggleWars.Spheres;
using UnityEditor.Animations;

namespace PeggleWars.Orbs
{
    internal class FireBombOrb : Orb, IAmSphere
    {
        [SerializeField] private Attack _fireBomb;
        [SerializeField] private GameObject _bombRadiusObject;
        [SerializeField] private AnimatorController _orb_Explosion;
        private readonly string TARGET_PARAM = "AOE_Target";

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<IAmSphere>(out _))
            {
                GetComponent<Collider2D>().enabled = false;
                AdditionalEffectsOnCollision();
                PlayOrbOnHitSound();
                OnCollisionVisualPolish();
                SpawnMana();
                ReplaceHitOrb();
                StartCoroutine(DestroyOrb());
            }
        }

        protected override void AdditionalEffectsOnCollision()
        {
            OrbActionManager.Instance.AddOrbToActionList(this);
            GetComponent<Animator>().runtimeAnimatorController = _orb_Explosion;
            _bombRadiusObject.SetActive(true);
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Will explode on impact, activating nearby orbs aswell. Enables the playes to cast a fire bomb, " +
                "hitting all enemies and setting them on fire.";
        }

        internal override IEnumerator OrbEffect()
        {
            GameObject aoeTargetObject = GameObject.FindGameObjectWithTag(TARGET_PARAM);
            _fireBomb.ShootAttack(aoeTargetObject.transform.position);
            yield return new WaitForSeconds(0.2f);
        }

        protected override IEnumerator DestroyOrb()
        {
            yield return new WaitForSeconds(1);
            Destroy(gameObject);
        }
    }
}
