using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/Orb Layout Set")]
    internal class OrbLayoutSet : ScriptableObject
    {
        [SerializeField] internal ScriptableOrbLayout[] OrbLayouts;
    }
}
