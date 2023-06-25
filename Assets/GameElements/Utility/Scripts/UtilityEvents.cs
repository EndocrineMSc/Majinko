using System;
using UnityEngine;

namespace Utility
{
    internal class UtilityEvents : MonoBehaviour
    {
        internal static event Action OnGameReset;

        internal static void RaiseGameReset()
        {
            OnGameReset?.Invoke();
        }
    }
}
