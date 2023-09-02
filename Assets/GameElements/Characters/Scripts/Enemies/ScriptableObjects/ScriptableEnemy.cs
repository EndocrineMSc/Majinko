using UnityEngine;


namespace Characters.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Enemy")]
    public class ScriptableEnemy : ScriptableObject
    {
        public int AttackFrequency;
        public EnemyAttackType AttackType;
        public int Damage;
        public int MaxHealth;
        public bool IsFlying;
        public bool IsStationary;
    }
}
