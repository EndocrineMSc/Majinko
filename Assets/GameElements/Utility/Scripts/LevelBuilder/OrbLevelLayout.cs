using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName ="Orb Levellayout")]
    public class OrbLevelLayout : ScriptableObject
    {
        [SerializeField] private Vector2[] _orbPositions;

        public Vector2[] OrbPositions
        {
            get { return _orbPositions; }
            private set { _orbPositions = value; }
        }

        public void WriteOrbPositions(Vector2[] positions)
        {
            OrbPositions = positions;
        }
    }
}
