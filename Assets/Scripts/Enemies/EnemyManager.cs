using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using PeggleWars.Enemies.Zombies;

namespace PeggleWars.Enemies
{
    /// <summary>
    /// This class handles movement and attack-initiation of enemies.
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        #region Fields and Properties

        public static EnemyManager Instance { get; private set; }

        [SerializeField] private Zombie _cloakedZombiePrefab;

        [SerializeField] private float monsterSpeed = 2;
        private readonly float finalDestination = 2f; //x where Enemy stops walking towards the player

        private List<Enemy> _enemiesInScene = new();
        public List<Enemy> EnemiesInScene { get => _enemiesInScene; set => _enemiesInScene = value; }

        private Enemy[] _enemyLibrary;

        #endregion

        #region Public Functions

        /// <summary>
        /// Spawns in enemies on the right side of the levelscreen.
        /// </summary>
        /// <param name="enemyType">Enum referencing different enemies alphabetically</param>
        public void SpawnEnemy(EnemyType enemyType)
        {
            Enemy tempEnemy = _enemyLibrary[(int)enemyType];
            Vector2 spawnPosition;
            
            if (tempEnemy.IsFlying)
            {
                spawnPosition = new Vector2(10.4f, 10.7f);
            }
            else
            {
                spawnPosition = new Vector2(10.4f, 5.7f);
            }

            Enemy instantiatedEnemy = Instantiate(tempEnemy, spawnPosition, Quaternion.identity); ;            
            EnemiesInScene.Add(instantiatedEnemy);           
        }

        //If enemy is in range to the player, attack
        public void MeleeEnemiesAttack()
        {
            for (int i = 0; i < EnemiesInScene.Count; i++)
            {
                Enemy enemy = EnemiesInScene[i];

                if (enemy.transform.position.x <= finalDestination && enemy.AttackType == EnemyAttackType.Melee)
                {
                    enemy.Attack();
                }
            }
        }

        public void RangedEnemiesAttack()
        {
            foreach (Enemy enemy in EnemiesInScene)
            {
                if (enemy.AttackType == EnemyAttackType.Distance)
                {
                    enemy.Attack();
                }
            }
        }

        /// <summary>
        /// Checks if enemy has arrived at the player, and moves enemies if there is space left of them.
        /// </summary>
        /// <returns>IEnumerator so that only one enemy moves at any given time, one after another</returns>
        public IEnumerator MoveAllEnemies()
        {
            foreach (Enemy enemy in EnemiesInScene)
            {
                bool needToMove = CheckForMoveNecessity(enemy);

                if (needToMove)
                {
                    yield return StartCoroutine(Move(enemy));
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
            }

            _enemyLibrary = Resources.LoadAll<Enemy>("EnemyPrefabs");
        }

        //Moves the enemy one "space" to the left
        private IEnumerator Move(Enemy enemy)
        {
            float startPosition = enemy.transform.position.x;
            float endPosition = Mathf.Round(enemy.transform.position.x - 2);
            float currentPosition = startPosition;
            
            enemy.GetComponent<Animator>().SetFloat("Speed", 1);

            Rigidbody2D _rigidbody = enemy.GetComponent<Rigidbody2D>();

            while (endPosition < currentPosition)
            {
                _rigidbody.velocity = Vector2.left * monsterSpeed;
                yield return new WaitForSeconds(0.1f);
                currentPosition = enemy.transform.position.x;
            }

            enemy.GetComponent<Animator>().SetFloat("Speed", 0);
            _rigidbody.velocity = Vector2.zero;
        }

        //enemies shouldn't move if the space left to them is blocked
        //ToDo: this needs to account for flying enemies aswell
        private bool CheckForMoveNecessity(Enemy enemy)
        {
            bool needsToMove;
            
            float currentPosition = enemy.transform.position.x;
            int index = EnemiesInScene.IndexOf(enemy);

            if (currentPosition <= finalDestination) //already at vampire position
            {
                needsToMove = false;
            }
            else if (index == 0) //if i == 0, => lefmost monster => monsterLeft is always null
            {
                needsToMove = true;
            }
            else
            {
                Enemy enemyLeft = EnemiesInScene[index - 1];
                float enemyLeftPosition = enemyLeft.transform.position.x;
                float distance = currentPosition - enemyLeftPosition;

                if (distance > 2.5f)
                {
                    needsToMove = true;
                }
                else
                {
                    needsToMove = false;
                }
            }
            return needsToMove;
        }

        #endregion
    }
}
