using System;
using UnityEngine;

namespace Utility
{
    internal class UtilityEvents : MonoBehaviour
    {
        internal static event Action OnGameReset;
        internal static event Action OnPlayerDeath;
        internal static event Action OnOverworldPlayerPositionChange;

        internal static void RaiseGameReset()
        {
            OnGameReset?.Invoke();
        }

        internal static void RaisePlayerDeath()
        {
            OnPlayerDeath?.Invoke();
        }

        internal static void RaiseOverWorldPlayerPositionChange()
        {
            OnOverworldPlayerPositionChange?.Invoke();
        }
    }
}
