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
