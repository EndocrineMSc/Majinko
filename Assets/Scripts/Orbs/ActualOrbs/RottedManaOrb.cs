using PeggleWars.ScrollDisplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleWars.Orbs
{
    internal class RottedManaOrb : Orb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, this orb spawns \"Rotted Mana\". When consumed to cast spells, lower player damage that turn.";
        }

        protected override void AdditionalEffectsOnCollision()
        {
            //not needed
        }

        internal override IEnumerator OrbEffect()
        {
            //not needed
            yield return null;
        }
    }
}
