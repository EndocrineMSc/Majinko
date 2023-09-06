using Orbs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Orbs
{
    public class OrbPositionWriter : MonoBehaviour
    {
        [SerializeField] private OrbLevelLayout _objectToWriteTo;

        // Start is called before the first frame update
        void Start()
        {
            var orbsInScene = GameObject.FindObjectsOfType<Orb>();

            Vector2[] orbPositions = new Vector2[orbsInScene.Length];

            for (int i = 0; i < orbPositions.Length; i++)
                orbPositions[i] = orbsInScene[i].transform.position;

            _objectToWriteTo.WriteOrbPositions(orbPositions);
        }
    }
}
