using Characters;
using Characters.UI;
using Characters.Enemies;
using UnityEngine;

namespace Attacks
{
    internal abstract class ProjectileAttack : Attack
    {
        #region Fields and Properties

        protected float _attackFlySpeed = 10;
        protected float _xInstantiateOffSet = 0.4f;

        #endregion

        #region Functions

        internal override void ShootAttack(Vector3 instantiatePosition, float damageModifier = 1)
        {
            if (EnemyManager.Instance.EnemiesInScene.Count > 0)
            {
                ProjectileAttack attack;

                if (_attackOrigin == AttackOrigin.Player)
                {
                    instantiatePosition = new Vector3((instantiatePosition.x + _xInstantiateOffSet), instantiatePosition.y, instantiatePosition.z);
                    attack = Instantiate(this, instantiatePosition, Quaternion.identity);
                    Rigidbody2D rigidbody = attack.GetComponent<Rigidbody2D>();
                    rigidbody.velocity = Vector3.right * _attackFlySpeed;
                    Player.Instance.GetComponent<PopUpSpawner>().SpawnPopUp(Bark);
                }
                else
                {
                    instantiatePosition = new Vector3((instantiatePosition.x - _xInstantiateOffSet), instantiatePosition.y, instantiatePosition.z);
                    attack = Instantiate(this, instantiatePosition, Quaternion.identity);
                    Rigidbody2D rigidbody = attack.GetComponent<Rigidbody2D>();
                    rigidbody.velocity = Vector3.left * _attackFlySpeed;
                    GetComponent<SpriteRenderer>().flipX = true;
                }

                attack.Damage = Mathf.CeilToInt(_attackValues.Damage * damageModifier);
            }
            else
            {
                Player.Instance.GetComponent<PopUpSpawner>().SpawnPopUp(NOTARGET_BARK);
            }
        }

        #endregion
    }
}