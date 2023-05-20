using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/AllOrbsCollection")]
    internal class AllOrbsCollection : ScriptableObject
    {
        [SerializeField] internal Orb[] AllOrbs;
    }
}
