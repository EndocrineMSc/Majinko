using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    internal class ArenaConditionTracker
    {
        internal static int AmountHitOrbsInTurn { get; private set; } = 0;

        internal static void OrbWasHit()
        {
            AmountHitOrbsInTurn++;
        }

        internal static void ResetHitOrbsInTurn()
        {
            AmountHitOrbsInTurn = 0;
        }
    }
}
