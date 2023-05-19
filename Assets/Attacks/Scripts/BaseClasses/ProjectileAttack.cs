using PeggleAttacks.AttackVisuals.PopUps;
using Enemies;
using UnityEngine;
using Characters;

namespace Attacks
{
    internal abstract class ProjectileAttack : Attack
    {
        #region Fields and Properties

        [SerializeField] protected float _attackFlySpeed = 10;
        protected float _xInstantiateOffSet = 0.4f;

        #endregion

        #region Functions

        internal override void ShootAttack(Vector3 instantiatePosition)
        {
            if (_attackOrigin == AttackOrigin.Player)
            {
                instantiatePosition = new Vector3((instantiatePosition.x + _xInstantiateOffSet), instantiatePosition.y, instantiatePosition.z);
            }
            else
            {
                instantiatePosition = new Vector3((instantiatePosition.x - _xInstantiateOffSet), instantiatePosition.y, instantiatePosition.z);
            }

            if (EnemyManager.Instance.EnemiesInScene.Count > 0)
            {
                Attack attack = Instantiate(this, instantiatePosition, Quaternion.identity);
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
                Player.Instance.GetComponent<PopUpSpawner>().SpawnPopUp(NOTARGET_BARK);
            }
        }

        #endregion
    }
}