using UnityEngine;

namespace Spheres
{
    [CreateAssetMenu(menuName = "Spheres/AllSpheresCollection")]
    internal class AllSpheresCollection : ScriptableObject
    {
        [SerializeField] internal Sphere[] AllSpheres;
    }
}
