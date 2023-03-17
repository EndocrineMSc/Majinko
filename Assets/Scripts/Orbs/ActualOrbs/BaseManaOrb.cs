using PeggleWars.ScrollDisplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleWars.Orbs
{
    internal class BaseManaOrb : Orb
    {
        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "Upon being hit, this orb spawns \"Basic Mana\". The most basic form of all magic orbs.";
        }

        internal override IEnumerator OrbEffect()
        {
            throw new System.NotImplementedException();
        }
    }
}
