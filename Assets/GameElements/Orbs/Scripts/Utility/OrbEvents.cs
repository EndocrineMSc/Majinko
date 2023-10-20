using EnumCollection;
using System;
using UnityEngine;

namespace Orbs
{
    internal class OrbEvents
    { 
        internal static event Action OnEffectEnd;
        internal static event Action<ManaType, int> SpawnMana;
        internal static event Action OnSetOrbsActive;
        internal static event Action<GameObject> OnOrbHit;

        internal static void RaiseEffectEnd()
        {
            OnEffectEnd?.Invoke();
        }

        internal static void RaiseSpawnMana(ManaType manaType, int amount)
        {
            SpawnMana?.Invoke(manaType, amount);
        }

        internal static void RaiseSetOrbsActive()
        {
            OnSetOrbsActive?.Invoke();
        }

        internal static void RaiseOrbHit(GameObject orb)
        {
            OnOrbHit?.Invoke(orb);
        }
    }
}
