using UnityEngine;
using UnityEngine.Events;

namespace PeggleWars.Cards
{
    internal class CardZoomEventHandler : MonoBehaviour
    {
        public UnityEvent<Vector3> CardZoomIn;
        public UnityEvent<Vector3> CardZoomOut;

        public void InvokeCardZoomIn(Vector3 position)
        {
            CardZoomIn?.Invoke(position);
        }

        public void InvokeCardZoomOut(Vector3 position)
        {
            CardZoomOut?.Invoke(position);
        }
    }
}
