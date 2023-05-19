using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(menuName = "Enemies/AllEnemiesCollection")]
    internal class AllEnemiesCollection : ScriptableObject
    {
        [SerializeField] internal Enemy[] AllEnemies;
    }
}
