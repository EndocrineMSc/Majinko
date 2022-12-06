using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using EnumCollection;
using Player;
using Enemies.Zombies;

namespace Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        #region Fields

        public static EnemyManager Instance { get; private set; }

        [SerializeField] private Zombie _cloakedZombie;
        private bool needToMove;
        [SerializeField] private float monsterSpeed = 2;
        private float finalDestination = 2f;

        public List<Enemy> Enemies = new List<Enemy>();

        #endregion

        #region Public Functions

        public void SpawnGroundEnemy(EnemyType enemyType)
        {
            Enemy tempEnemy = null;
            switch (enemyType)
            {
                case EnemyType.CloakedZombie:
                    tempEnemy = Instantiate(_cloakedZombie, new Vector2(10f, -8.93f), Quaternion.identity);
                    break;
            }

            Enemies.Add(tempEnemy);           
        }


        public void MeleeEnemiesAttack()
        {
            Enemy enemy = Enemies[0];
            if (enemy != null)
            {
                float enemyPosition = enemy.transform.position.x;
                if (enemyPosition <= finalDestination && enemy.AttackType == EnemyAttackType.Melee)
                {
                    enemy.GetComponent<Animator>().SetTrigger("Attack");

                    int damage = enemy.Damage;
                    Vampire.Nosferatu.TakeDamage(damage);                 
                }
            }          
        }

        public void RangedEnemiesAttack()
        {
            foreach (Enemy enemy in Enemies)
            {
                if (enemy != null)
                {
                    if (enemy.AttackType == EnemyAttackType.Distance)
                    {
                        enemy.GetComponent<Animator>().SetTrigger("Attack");

                        int damage = enemy.Damage;
                        Vampire.Nosferatu.TakeDamage(damage);
                    }
                }
            }
        }


        //removes an enemy from the global list, and destroys it after 1 second
        public void KillEnemy()
        {
            foreach (Enemy enemy in Enemies)
            {
                if (enemy.Health <= 0)
                {
                    Enemy tempEnemy = enemy;
                    int index = Enemies.IndexOf(enemy);
                    Enemies.RemoveAt(index);
                    StartCoroutine(EnemyDeath(tempEnemy));
                }
            }
        }

        #endregion

        #region Private Functions

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        #endregion

        #region IEnumerators

        public IEnumerator MoveRightEnemies()
        {
            foreach (Enemy enemy in Enemies)
            {
                float currentPosition = enemy.transform.position.x;
                int index = Enemies.IndexOf(enemy);

                if (currentPosition <= finalDestination) //already at vampire position
                {
                    needToMove = false;
                }
                else if (index == 0) //if i == 0, => lefmost monster => monsterLeft is always null
                {
                    needToMove = true;
                }
                else
                {
                    Enemy enemyLeft = Enemies[index - 1];
                    float enemyLeftPosition = enemyLeft.transform.position.x;
                    float distance = currentPosition - enemyLeftPosition;

                    if (distance > 2.5f)
                    {
                        needToMove = true;
                    }
                    else
                    {
                        needToMove = false;
                    }
                }

                if (needToMove)
                {
                    enemy.GetComponent<Animator>().SetFloat("Speed", 1);
                    yield return StartCoroutine(MoveLeft(enemy));
                    enemy.GetComponent<Animator>().SetFloat("Speed", 0);
                    needToMove = false;
                }
            }
            needToMove = false;
        }

        private IEnumerator MoveLeft(Enemy enemy)
        {
            float startPosition = enemy.transform.position.x;
            float endPosition = Mathf.Round(enemy.transform.position.x - 2);
            float currentPosition = startPosition;
            Rigidbody2D _rigidbody = enemy.GetComponent<Rigidbody2D>();

            while (endPosition < currentPosition)
            {
                _rigidbody.velocity = Vector2.left * monsterSpeed;
                yield return new WaitForSeconds(0.1f);
                currentPosition = enemy.transform.position.x;
            }

            _rigidbody.velocity = Vector2.zero;
            yield break;
        }

        private IEnumerator EnemyDeath(Enemy enemy)
        {
            yield return new WaitForSeconds(1f);
            Destroy(enemy);
        }
        #endregion

    }
}
