using PeggleWars.Enemies;
using PeggleWars.Orbs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PeggleWars.Attacks.Attack;


namespace PeggleWars.Attacks
{
    internal abstract class InstantAttack : Attack
    {
        #region Fields and Properties

        [SerializeField] protected float _timeOfExistance;

        #endregion

        #region Functions

        internal override void ShootAttack()
        {
            if (EnemyManager.Instance.EnemiesInScene.Count > 0)
            {
                Instantiate(this, _instantiatePosition, Quaternion.identity);
            }
        }

        protected override void DestroyGameObject()
        {
            StartCoroutine(WaitForAnimationEnd());
        }

        protected IEnumerator WaitForAnimationEnd()
        {
            yield return new WaitForSeconds(_timeOfExistance);
            Destroy(gameObject);
        }

        #endregion
    }
}