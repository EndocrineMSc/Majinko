using System;
using UnityEngine;

namespace Cards
{
    internal class CardEvents
    {
        internal static event Action<Vector3> OnCardZoomIn;
        internal static event Action OnCardZoomOut;
        internal static event Action OnCardDisabled;
        internal static bool CardIsZoomed { get; private set; }

        internal static void RaiseCardZoomIn(Vector3 position)
        {
            OnCardZoomIn?.Invoke(position);
            CardIsZoomed = true;
        }

        internal static void InvokeCardZoomOut()
        {
            OnCardZoomOut?.Invoke();
            CardIsZoomed = false;
        }

        internal static void RaiseCardDisabled()
        {
            OnCardDisabled?.Invoke();
            CardIsZoomed = false;
        }
    }
}
