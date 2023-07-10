using EnumCollection;
using System;

namespace Orbs
{
    internal class OrbEvents
    { 
        internal static event Action OnEffectEnd;
        internal static event Action<ManaType, int> SpawnMana;
        internal static event Action OnSetOrbsActive;

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
    }
}
