using System;
using UnityEngine;

namespace Spheres
{
    internal class SphereEvents : MonoBehaviour
    {
        internal static event Action OnSphereStacked;
        internal static event Action OnSphereDestruction;
        
        internal static void RaiseSphereStacked()
        {
            OnSphereStacked?.Invoke();
        }

        internal static void RaiseSphereDestruction()
        {
            OnSphereDestruction?.Invoke();
        }
    }
}
