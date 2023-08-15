using Audio;
using Characters.Enemies;
using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attacks
{
    internal abstract class AreaAttack : InstantAttack
    {
        public override string Bark { get; } = "Fire Bomb!";

        internal override void ShootAttack(Vector3 instantiatePosition, float damageModifier = 1)
        {
            AreaAttack attack = Instantiate(this, instantiatePosition, Quaternion.identity);
            attack.HandleAOE(damageModifier);
        }

        protected virtual void HandleAOE(float damageModifier)
        {
            IterateEnemiesForDamage(damageModifier);
        }

        protected void IterateEnemiesForDamage(float damageModifier)
        {
            int amountEnemies = EnemyManager.Instance.EnemiesInScene.Count;
            List<Enemy> enemyList = EnemyManager.Instance.EnemiesInScene;
            Damage = Mathf.CeilToInt(Damage * damageModifier);

            for (int i = 0; i < amountEnemies; i++)
            {
                if (i < EnemyManager.Instance.EnemiesInScene.Count)
                {
                    Enemy currentEnemy = enemyList[i];
                    if (currentEnemy != null)
                    {
                        bool isIntangible = false;

                        if (currentEnemy.TryGetComponent(out ICanBeIntangible intangibleEnemy))
                            isIntangible = intangibleEnemy.IntangibleStacks > 0;

                        if (!isIntangible)
                        {
                            DealDamage(currentEnemy);
                            AdditionalDamageEffects(currentEnemy.gameObject);
                        }
                    }
                }
            }
        }

        protected virtual void DealDamage(Enemy enemy)
        {
            enemy.TakeDamage(Damage);
        }

        protected override void PlayHitSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }

        protected override void PlayAwakeSound()
        {
            //only one sound needed for this special attack (hit sound)
        }
    }
}
