using Audio;
using Characters.Enemies;
using UnityEngine;
using PeggleWars.Attacks;
using Characters;
using System.Collections;
using System.Collections.Generic;

namespace Attacks
{
    internal class FireBomb : InstantAttack
    {
        public override string Bark { get; } = "Fire Bomb!";

        //Do special stuff in here
        protected override void Awake()
        {
            base.Awake();
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
            DestroyGameObject();
        }

        internal override void ShootAttack(Vector3 instantiatePosition, float damageModifier = 1)
        {
            FireBomb currentBomb = Instantiate(this, instantiatePosition, Quaternion.identity);
            currentBomb.HandleAOE(damageModifier);
        }

        protected override void OnHitPolish()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }

        public void HandleAOE(float damageModifier)
        {
            StartCoroutine(InstantiateExplosions(damageModifier));
        }

        protected override void AdditionalEffectsOnImpact()
        {
            //empty
        }

        protected IEnumerator InstantiateExplosions(float damageModifier)
        {
            int xIndexInEnemyPositions = GetEnemyPositionIndex();
            int rightMostEnemyPosition = EnemyManager.Instance.EnemyPositions.GetLength(1) - 1;

            if (xIndexInEnemyPositions < rightMostEnemyPosition)
            {
                Vector2 nextPosition = EnemyManager.Instance.EnemyPositions[0, xIndexInEnemyPositions + 1];
                yield return new WaitForSeconds(_timeOfExistance / 3);
                ShootAttack(nextPosition);
            }
            else
            {
                DealExplosionDamage(damageModifier);
            }
        }

        protected void DealExplosionDamage(float damageModifier)
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
                        {
                            isIntangible = intangibleEnemy.IntangibleStacks > 0;
                        }

                        if (!isIntangible)
                        {
                            currentEnemy.TakeDamage(Damage);
                            currentEnemy.ApplyBurning(_attackValues.BurningStacks);
                        }
                    }
                }
            }
        }

        private int GetEnemyPositionIndex()
        {
            Vector2 currentPosition = transform.position;
            for (int i = 0; i < EnemyManager.Instance.EnemyPositions.Length; i++)
            {
                Vector2 indexPosition = EnemyManager.Instance.EnemyPositions[0, i];
                if (indexPosition.Equals(currentPosition))
                {
                    return i;
                }
            }
            Debug.Log("Enemy Position for Fire Bomb not found");
            return -1;
        }
    }
}
