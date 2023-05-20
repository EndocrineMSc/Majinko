using UnityEngine;
using EnumCollection;

namespace Characters.Enemies
{
    [CreateAssetMenu]
    internal class ScriptableEnemySet : ScriptableObject
    {
        [SerializeField] private EnemyType[] _enemyArray;
        internal EnemyType[] EnemyArray { get => _enemyArray; }
    }
}
