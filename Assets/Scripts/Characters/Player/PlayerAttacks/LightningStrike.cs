using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Enemies;
using EnumCollection;
using PeggleWars.Orbs;

namespace PeggleWars.PlayerAttacks
{
    internal class LightningStrike : PlayerAttack
    {
        #region Fields and Properties

        [SerializeField] private float _animationDuration;

        #endregion

        #region Functions

        protected override void Start()
        {
            base.Start();
            EnemyManager enemyManager = EnemyManager.Instance;
            _target = PlayerAttackTarget.LastEnemy;
            
            if (enemyManager.EnemiesInScene.Count > 0 )
            {
                _instantiatePosition = enemyManager.EnemiesInScene[enemyManager.EnemiesInScene.Count - 1].transform.position;
            }
            else
            {
                _instantiatePosition = Vector2.zero;
            }
        }

        protected override void OnHitPolish()
        {
            //ToDo Play Sound
        }

        protected override void DestroyGameObject()
        {
            StartCoroutine(WaitForAnimationEnd(_animationDuration));
        }

        private IEnumerator WaitForAnimationEnd(float duration)
        {
            yield return new WaitForSeconds(duration);
            OrbEvents.Instance.OrbEffectEnd?.Invoke();
            Destroy(gameObject);
        }

        internal override void ShootAttack(PlayerAttack playerAttack)
        {
            EnemyManager enemyManager = EnemyManager.Instance;
            
            if (enemyManager.EnemiesInScene.Count > 0)
            {
                _instantiatePosition = enemyManager.EnemiesInScene[(enemyManager.EnemiesInScene.Count - 1)].transform.position;
                Instantiate(playerAttack, _instantiatePosition, Quaternion.identity);
            }
            else
            {
                _instantiatePosition = Vector2.zero;
                OrbEvents.Instance.OrbEffectEnd?.Invoke();
            }
        }

        protected override void AdditionalEffectsOnImpact()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
