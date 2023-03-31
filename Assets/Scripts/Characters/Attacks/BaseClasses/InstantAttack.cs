using PeggleAttacks.AttackVisuals.PopUps;
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

        internal override void ShootAttack(Vector3 instantiatePosition)
        {
            if (EnemyManager.Instance.EnemiesInScene.Count > 0)
            {
                Instantiate(this, instantiatePosition, Quaternion.identity);
                Player.Instance.GetComponent<PopUpSpawner>().SpawnPopUp(Bark);
            }
            else
            {
                Player.Instance.GetComponent<PopUpSpawner>().SpawnPopUp(NO_TARGET_PARAM);
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