using Characters;
using Characters.UI;
using Characters.Enemies;
using UnityEngine;
using Orbs;
using PeggleWars.Characters.Interfaces;

namespace Attacks
{
    internal abstract class ProjectileAttack : Attack
    {
        #region Fields and Properties

        protected float _attackFlySpeed = 25;
        protected float _xInstantiateOffSet = 0.4f;
        protected readonly string BORDER_TAG = "ProjectileBorder";

        #endregion

        #region Functions

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            HandleAttackHit(collision.gameObject);
        }

        protected virtual void HandleAttackHit(GameObject target)
        {
            if ((_attackOrigin == AttackOrigin.Player && target.GetComponent<Enemy>() != null)
                || _attackOrigin == AttackOrigin.Enemy && target.GetComponent<Player>() != null)
            {
                IDamagable damagableTarget = target.GetComponent<IDamagable>();
                damagableTarget?.TakeDamage(Damage);
                OnHitPolish(Damage);
                AdditionalDamageEffects(target);

                if (_attackOrigin == AttackOrigin.Player)
                    RaiseAttackFinished();

                Destroy(gameObject);
            }

            if (target.CompareTag(BORDER_TAG))
            {
                RaiseAttackFinished();
                Destroy(gameObject);
            }
        }

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
                    attack.Damage = Mathf.FloorToInt(_attackValues.Damage * PlayerAttackDamageManager.Instance.DamageModifierTurn);
                }
                else
                {
                    instantiatePosition = new Vector3((instantiatePosition.x - _xInstantiateOffSet), instantiatePosition.y, instantiatePosition.z);
                    attack = Instantiate(this, instantiatePosition, Quaternion.identity);
                    Rigidbody2D rigidbody = attack.GetComponent<Rigidbody2D>();
                    rigidbody.velocity = Vector3.left * _attackFlySpeed;
                    attack.Damage = Mathf.CeilToInt(_attackValues.Damage * damageModifier);
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