using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


namespace BloodBehaviuor
{
    public class Orb : MonoBehaviour
    {
        #region Fields

        [SerializeField] protected Mana BasicMana;
        [SerializeField] protected ManaType ManaType;
        [SerializeField] protected int BaseManaAmount = 10;
        private GameObject[] _spawnArray;
        private float _spawnOffset = 0.02f;

        #endregion

        #region Properties




        #endregion

        #region Public Functions





        #endregion

        #region Private Functions

        private void Start()
        {
            //Ignore the collisions between layer 0 (default) and layer 8 (custom layer you set in Inspector window)
            Physics.IgnoreLayerCollision(6, 7);

            _spawnArray = GameObject.FindGameObjectsWithTag("ManaSpawn");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains("Shot"))
            {
                gameObject.GetComponent<SpriteRenderer>().size += new Vector2(0.03f, 0.03f);
                StartCoroutine(nameof(SetInactive));

                SpawnMana(BasicMana, BaseManaAmount);
            }
        }

        #endregion

        #region Protected Virtual Functions

        protected virtual void SpawnMana(Mana OrbMana, int Amount)
        {
            //spawns i mana particles in a roughly ball shaped form -> to enable shader blur, visualizing the blood properly
            for (int i = 0; i < Amount; i++)
            {
                if ((i % 2) == 0 && i < Amount/2)
                {
                    Mana InstanceMana = Instantiate(OrbMana, new Vector3(transform.position.x + (i * _spawnOffset), transform.position.y - (i * _spawnOffset), transform.position.z), Quaternion.identity);
                    StartCoroutine(RelocateMana(InstanceMana));
                }
                else if ((i % 2) == 1 && i < Amount/2)
                {
                    Mana InstanceMana = Instantiate(OrbMana, new Vector3(transform.position.x - (i * _spawnOffset), transform.position.y - (i * _spawnOffset), transform.position.z), Quaternion.identity);
                    StartCoroutine(RelocateMana(InstanceMana));
                }
                else if ((i % 2) == 0 && i >= Amount/2)
                {
                    Mana InstanceMana = Instantiate(OrbMana, new Vector3(transform.position.x + ((9 - i) * _spawnOffset), transform.position.y - ((9 - i) * _spawnOffset), transform.position.z), Quaternion.identity);
                    StartCoroutine(RelocateMana(InstanceMana));
                }
                else
                {
                    Mana InstanceMana = Instantiate(OrbMana, new Vector3(transform.position.x - ((9 - i) * _spawnOffset), transform.position.y - ((9 - i) * _spawnOffset), transform.position.z), Quaternion.identity);
                    StartCoroutine(RelocateMana(InstanceMana));
                }
            }
        }


        #endregion

        #region IEnumerators

        private IEnumerator SetInactive()
        {
            yield return new WaitForSeconds(0.1f);
            gameObject.GetComponent<SpriteRenderer>().size -= new Vector2(0.02f, 0.02f);
            gameObject.SetActive(false);
        }

        private IEnumerator RelocateMana(Mana InstanciatedMana)
        {
           
            yield break;
        }

        #endregion

    }
}

