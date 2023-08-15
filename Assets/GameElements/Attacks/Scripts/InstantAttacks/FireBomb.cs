using Audio;
using Characters.Enemies;
using UnityEngine;
using System.Collections;

namespace Attacks
{
    internal class FireBomb : AreaAttack
    {
        public override string Bark { get; } = "Fire Bomb!";

        protected override void HandleAOE(float damageModifier)
        {
            StartCoroutine(InstantiateExplosions(damageModifier));
        }

        protected IEnumerator InstantiateExplosions(float damageModifier)
        {
            int xIndexInEnemyPositions = GetEnemyPositionIndex();
            int rightMostEnemyPosition = EnemyManager.Instance.EnemyPositions.GetLength(1) - 1;

            if (xIndexInEnemyPositions < rightMostEnemyPosition)
            {
                Vector2 nextPosition = EnemyManager.Instance.EnemyPositions[0, xIndexInEnemyPositions + 1];
                OnHitPolish(damageModifier * _attackValues.Damage);
                yield return new WaitForSeconds(_timeOfExistance / 3);
                ShootAttack(nextPosition);
            }
            else
            {
                IterateEnemiesForDamage(damageModifier);
                RaiseAttackFinished();
            }
        }

        private int GetEnemyPositionIndex()
        {
            Vector2 currentPosition = transform.position;
            for (int i = 0; i < EnemyManager.Instance.EnemyPositions.Length; i++)
            {
                Vector2 indexPosition = EnemyManager.Instance.EnemyPositions[0, i];
                
                if (indexPosition.Equals(currentPosition))
                    return i;               
            }
            Debug.Log("Enemy Position for Fire Bomb not found");
            return -1;
        }

        protected override void PlayHitSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0111_FireBomb);
        }

        protected override void PlayAwakeSound()
        {
            //only one sound needed for this special attack (hit sound)
        }

        protected override void AdditionalDamageEffects(GameObject target)
        {
            if (target.TryGetComponent<Enemy>(out var enemy))
                enemy.ApplyBurning(_attackValues.BurningStacks);
        }

        //Moved RaiseAttackFinished to Explosionhandling,
        //only last explosion triggers the event now
        protected override IEnumerator HandleLifeTime()
        {
            yield return new WaitForSeconds(_timeOfExistance);
            Destroy(gameObject);
        }
    }
}
