using PeggleAttacks.AttackVisuals.PopUps;
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
        [SerializeField] protected float _xInstantiateOffSet = 5f;

        #endregion

        #region Functions

        internal override void SetAttackInstantiatePosition(Transform originTransform)
        {
            float enemyPositionsY = EnemyManager.Instance.EnemyPositions[0, 0].y;
            if (_attackOrigin == AttackOrigin.Player)
            {
                _instantiatePosition = new Vector2(originTransform.position.x + _xInstantiateOffSet, enemyPositionsY);
            }
            else
            {
                _instantiatePosition = new Vector2(originTransform.position.x - _xInstantiateOffSet, enemyPositionsY);
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
                    Player.Instance.GetComponent<PopUpSpawner>().SpawnPopUp(Bark);
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    rigidbody.velocity = Vector3.left * _attackFlySpeed;
                }
            }
            else
            {
                Player.Instance.GetComponent<PopUpSpawner>().SpawnPopUp(_noTargetString);
            }
        }

        #endregion
    }
}