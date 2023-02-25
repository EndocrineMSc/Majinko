using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PeggleWars.Enemies
{
    [CreateAssetMenu]
    public class ScriptableEnemySet : ScriptableObject
    {
        [SerializeField] private EnemyType[] _enemyArray;
        public EnemyType[] EnemyArray { get => _enemyArray; }
    }
}
