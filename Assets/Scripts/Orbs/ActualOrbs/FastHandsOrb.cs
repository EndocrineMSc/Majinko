using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Cards.DeckManagement.HandHandling;

namespace PeggleWars.Orbs
{
    public class FastHandsOrb : Orb
    {
        public override IEnumerator OrbEffect()
        {
            Hand.Instance.DrawAmount += 1;
            yield return null;
        }

        protected override void AdditionalEffectsOnCollision()
        {
            StartCoroutine(OrbEffect());
        }
    }
}
