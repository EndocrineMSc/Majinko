using UnityEngine;
using UnityEngine.Events;

namespace Cards
{
    internal class CardZoomEventHandler : MonoBehaviour
    {
        public static UnityEvent<Vector3> CardZoomIn { get; private set; }
        public static UnityEvent CardZoomOut { get; private set; }

        public void Awake()
        {
            CardZoomIn ??= new UnityEvent<Vector3>();
            CardZoomOut ??= new UnityEvent();
        }

        public static void InvokeCardZoomIn(Vector3 position)
        {
            CardZoomIn?.Invoke(position);
        }

        public static void InvokeCardZoomOut()
        {
            CardZoomOut?.Invoke();
        }
    }
}
