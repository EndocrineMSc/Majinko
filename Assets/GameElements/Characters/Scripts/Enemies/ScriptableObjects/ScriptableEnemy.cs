using Audio;
using UnityEngine;


namespace Characters.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Enemy")]
    public class ScriptableEnemy : ScriptableObject
    {
        [TextArea] public string Description;
        public int AttackFrequency;
        public EnemyAttackType AttackType;
        public int Damage;
        public int MaxHealth;
        public bool IsRanged;
        public bool IsFlying;
        public bool IsStationary;
        public SFX SpawnSound;
        public SFX DeathSound;
        public SFX HurtSound;
        public EnemyAttackEffect AttackEffect;
        public EnemyDeathEffect DeathEffect;
    }
}
