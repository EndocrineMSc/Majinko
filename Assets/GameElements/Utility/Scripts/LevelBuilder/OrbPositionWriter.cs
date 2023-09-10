using System.Collections.Generic;
using UnityEngine;

namespace Orbs
{
    public class OrbPositionWriter : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var orbsInScene = GameObject.FindObjectsOfType<Orb>();

            List<Vector2> orbPositions = new();

            foreach (var orb in orbsInScene)
            {
                orbPositions.Add(orb.transform.position);
            }

            var newLayout = OrbLevelLayout.CreateLayout(orbPositions);
            UnityEditor.AssetDatabase.CreateAsset(newLayout, "Assets/NewLayout.asset");
        }
    }
}
