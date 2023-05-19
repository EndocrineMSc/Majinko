using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Enemy")]
    internal class ScriptableEnemy : ScriptableObject
    {
        [SerializeField] internal int AttackFrequency;
        [SerializeField] internal EnemyAttackType AttackType;
        [SerializeField] internal int Damage;
        [SerializeField] internal int MaxHealth;
        [SerializeField] internal bool IsFlying;
    }
}
