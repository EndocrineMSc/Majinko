using UnityEngine;
using EnumCollection;

namespace Characters.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/EnemySet")]
    internal class ScriptableEnemySet : ScriptableObject
    {
        [SerializeField] private GameObject[] _enemyPrefabs;
        internal GameObject[] EnemyArray { get => _enemyPrefabs; }
    }
}
