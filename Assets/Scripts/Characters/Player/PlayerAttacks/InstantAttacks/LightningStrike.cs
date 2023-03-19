using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Enemies;
using EnumCollection;
using PeggleWars.Orbs;

namespace PeggleWars.Attacks
{
    internal class LightningStrike : InstantAttack
    {
        public override string Bark { get; } = "Lightning Strike!";

        protected override void OnHitPolish()
        {
            //ToDo Play Sound
        }

        protected override void AdditionalEffectsOnImpact()
        {
            //empty
        }
    }
}
