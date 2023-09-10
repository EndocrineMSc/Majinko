using System.Collections.Generic;
using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName ="Orb Levellayout")]
    public class OrbLevelLayout : ScriptableObject
    {
        [SerializeField] private List<Vector2> _orbPositions = new();

        public List<Vector2> OrbPositions
        {
            get { return _orbPositions; }
            private set { _orbPositions = value; }
        }

        public void Init(List<Vector2> orbPositions)
        {
            this._orbPositions = orbPositions;
        }

        public static OrbLevelLayout CreateLayout(List<Vector2> positions)
        {
            var layout = ScriptableObject.CreateInstance<OrbLevelLayout>();
            layout.Init(positions);
            return layout;
        }
    }
}
