using PeggleWars.ScrollDisplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Attacks;

namespace PeggleWars.Orbs
{
    internal class FireBombOrb : Orb
    {
        [SerializeField] private Attack _fireBomb;
        [SerializeField] private GameObject bombRadiusObject;

        protected override void AdditionalEffectsOnCollision()
        {
            bombRadiusObject.SetActive(true);
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Will explode on impact, activating nearby orbs aswell. Enables the playes to cast a fire bomb, " +
                "hitting all enemies and setting them on fire.";
        }

        internal override IEnumerator OrbEffect()
        {
            _fireBomb.ShootAttack();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
