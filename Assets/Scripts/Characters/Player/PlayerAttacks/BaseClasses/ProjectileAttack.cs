using PeggleWars.Enemies;
using PeggleWars.Orbs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleWars.Attacks
{
    internal abstract class ProjectileAttack : Attack
    {
        #region Fields and Properties

        [SerializeField] protected float _attackFlySpeed = 10;
        [SerializeField] protected float _xInstantiateOffSet = 1f;
        [SerializeField] protected AttackOrigin _attackOrigin;

        #endregion

        #region Functions

        internal override void SetAttackInstantiatePosition(Transform targetTransform)
        {
            if (_attackOrigin == AttackOrigin.Player)
            {
                _instantiatePosition = new Vector2(targetTransform.position.x + _xInstantiateOffSet, targetTransform.position.y);
            }
            else
            {
                _instantiatePosition = new Vector2(targetTransform.position.x -  _xInstantiateOffSet, targetTransform.position.y);
            }           
        }

        internal override void ShootAttack()
        {
            if (EnemyManager.Instance.EnemiesInScene.Count > 0)
            {
                Attack attack = Instantiate(this, _instantiatePosition, Quaternion.identity);
                Rigidbody2D rigidbody = attack.GetComponent<Rigidbody2D>();

                if (_attackOrigin == AttackOrigin.Player)
                {
                    rigidbody.velocity = Vector3.right * _attackFlySpeed;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    rigidbody.velocity = Vector3.left * _attackFlySpeed;
                }
            }           
        }

        #endregion
    }
}
