using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using EnumCollection;
using Player;

namespace Monsters
{
    public class MonsterManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private CloakedZombie _cloakedZombie;
        private bool needToMove;
        [SerializeField] private float monsterSpeed = 2;
        private float finalDestination = 2f;

        #endregion

        #region Public Functions

        public void SpawnZombie()
        {      
            Instantiate(_cloakedZombie, new Vector2(10f, -8.93f), Quaternion.identity);
        }

        public void MeleeMonstersAttack(GameObject[] monsters)
        {
            GameObject monster = monsters[0];
            float monsterPosition = monster.transform.position.x;
            if (monsterPosition <= finalDestination)
            {
                monster.GetComponent<Animator>().SetTrigger("Attack");

                string monsterName = monster.name;

                if (monsterName.Contains(EnemyType.CloakedZombie.ToString()))
                {
                    CloakedZombie cloakedZombie = monster.GetComponent<CloakedZombie>();
                    int damage = cloakedZombie.Damage;
                    Vampire.Nosferatu.TakeDamage(damage);
                }
            }
        }


        public IEnumerator MoveRightMonsters(GameObject[] monsters)
        {
            for (int i = 0; i < monsters.Length; i++)
            {
                GameObject monster = monsters[i];
                float currentPosition = monster.transform.position.x;

                if (currentPosition <= finalDestination) //already at vampire position
                {
                    needToMove = false;
                }
                else if (i == 0) //if i == 0, => lefmost monster => monsterLeft is always null
                {
                    needToMove = true;
                }
                else
                {
                    GameObject monsterLeft = monsters[i - 1];
                    float monsterLeftPosition = monsterLeft.transform.position.x;
                    float distance = currentPosition - monsterLeftPosition;

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
                    monster.GetComponent<Animator>().SetFloat("Speed", 1);
                    yield return StartCoroutine(MoveLeft(monster));
                    monster.GetComponent<Animator>().SetFloat("Speed", 0);
                    needToMove = false;
                }
            }
            needToMove = false;
        }

        private IEnumerator MoveLeft(GameObject monster)
        {
            float startPosition = monster.transform.position.x;
            float endPosition = Mathf.Round(monster.transform.position.x - 2);
            float currentPosition = startPosition;
            Rigidbody2D _rigidbody = monster.GetComponent<Rigidbody2D>();

            while (endPosition < currentPosition)
            {
                _rigidbody.AddForce(Vector2.left * monsterSpeed);
                yield return new WaitForSeconds(0.1f);
                currentPosition = monster.transform.position.x;
            }

            _rigidbody.velocity = Vector2.zero;
            yield break;
        }

        #endregion

    }
}
