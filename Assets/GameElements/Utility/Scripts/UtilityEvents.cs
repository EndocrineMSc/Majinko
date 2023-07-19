using System;
using UnityEngine;

namespace Utility
{
    internal class UtilityEvents : MonoBehaviour
    {
        internal static event Action OnGameReset;
        internal static event Action OnLevelVictory;
        internal static event Action OnPlayerDeath;
        internal static event Action OnOverworldPlayerPositionChange;
        internal static event Action<GameObject> OnDisplayOnScrollTrigger;
        internal static event Action OnStopScrollDisplayTrigger;


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

        internal static void RaiseLevelVictory()
        {
            OnLevelVictory?.Invoke();
        }

        internal static void RaiseDisplayOnScroll (GameObject gameObject)
        {
            OnDisplayOnScrollTrigger?.Invoke(gameObject);
        }

        internal static void RaiseStopScrollDisplay()
        {
            OnStopScrollDisplayTrigger?.Invoke();
        }
    }
}
