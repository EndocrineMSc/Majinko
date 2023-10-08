using System.Collections.Generic;
using UnityEngine;

namespace Orbs
{
    public class OrbPositionWriter : MonoBehaviour
    {
        [SerializeField] private OrbLevelLayout _layoutToModify;
        [SerializeField] private GameObject _basicOrbPrefab;

        // Start is called before the first frame update
        void Start()
        {
            if (_layoutToModify != null)
                foreach (var pos in _layoutToModify.OrbPositions)
                    Instantiate(_basicOrbPrefab, pos, Quaternion.identity);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                CreateNewLayout();
        }

        private void CreateNewLayout()
        {
            var orbsInScene = GameObject.FindObjectsOfType<Orb>();

            List<Vector2> orbPositions = new();

            foreach (var orb in orbsInScene)
            {
                orbPositions.Add(orb.transform.position);
            }

            var newLayout = OrbLevelLayout.CreateLayout(orbPositions);
            //UnityEditor.AssetDatabase.CreateAsset(newLayout, "Assets/NewLayout.asset");
        }
    }
}
