using System;
using UnityEngine;

namespace Cards
{
    internal class CardEvents
    {
        internal static event Action<Vector3> OnCardZoomIn;
        internal static event Action OnCardZoomOut;
        internal static event Action OnCardDestruction;

        internal static void RaiseCardZoomIn(Vector3 position)
        {
            OnCardZoomIn?.Invoke(position);
        }

        internal static void InvokeCardZoomOut()
        {
            OnCardZoomOut?.Invoke();
        }

        internal static void RaiseCardDestruction()
        {
            OnCardDestruction?.Invoke();
        }
    }
}
