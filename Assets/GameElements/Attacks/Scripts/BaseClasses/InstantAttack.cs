using Characters;
using Characters.UI;
using Characters.Enemies;
using System.Collections;
using UnityEngine;

namespace Attacks
{
    internal abstract class InstantAttack : Attack
    {
        #region Fields and Properties

        [SerializeField] protected float _timeOfExistance;

        #endregion

        #region Functions

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(WaitForAnimationEnd());
        }

        internal override void ShootAttack(Vector3 instantiatePosition, float damageModifier = 1)
        {
            if (EnemyManager.Instance.EnemiesInScene.Count > 0)
            {
                InstantAttack attack = Instantiate(this, instantiatePosition, Quaternion.identity);               

                if (_attackOrigin == AttackOrigin.Player)
                {
                    Player.Instance.GetComponent<PopUpSpawner>().SpawnPopUp(Bark);
                    attack.Damage = Mathf.FloorToInt(_attackValues.Damage * PlayerAttackDamageManager.Instance.DamageModifierTurn);
                }
                else
                {
                    attack.Damage = Mathf.CeilToInt(_attackValues.Damage * damageModifier);
                }
            }
            else
            {
                Player.Instance.GetComponent<PopUpSpawner>().SpawnPopUp(NOTARGET_BARK);
            }
        }

        protected IEnumerator WaitForAnimationEnd()
        {
            yield return new WaitForSeconds(_timeOfExistance);
            Destroy(gameObject);
        }

        #endregion
    }
}