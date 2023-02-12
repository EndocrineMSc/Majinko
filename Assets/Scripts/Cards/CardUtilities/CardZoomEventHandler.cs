using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cards.Zoom
{
    public class CardZoomEventHandler : MonoBehaviour
    {
        public UnityEvent<Vector3> CardZoomIn;
        public UnityEvent<Vector3> CardZoomOut;

        public void InvokeCardZoomIn(Vector3 position)
        {
            Debug.Log("ZoomCard position: " + position.x);
            CardZoomIn?.Invoke(position);
        }

        public void InvokeCardZoomOut(Vector3 position)
        {
            CardZoomOut?.Invoke(position);
        }

    }
}
