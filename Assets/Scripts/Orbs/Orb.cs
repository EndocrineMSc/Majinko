using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using PeggleMana;


namespace PeggleOrbs
{
    public class Orb : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Mana _basicMana;
        [SerializeField] private Mana _fireMana;
        [SerializeField] private Mana _iceMana;
        [SerializeField] private Mana _lightningMana;
        [SerializeField] private Mana _darkMana;
        [SerializeField] private Mana _lightMana;
        [SerializeField] private ManaType SpawnManaType;
        [SerializeField] protected int ManaAmount = 10;
        private GameObject[] SpawnArray;
        private Mana[] ManaTypes;

        private ManaPoolManager _manaPoolManager;

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

            SpawnArray = GameObject.FindGameObjectsWithTag("ManaSpawn");
            ManaTypes = new Mana[] {_basicMana, _fireMana, _iceMana, _lightningMana, _darkMana, _lightMana};
            _manaPoolManager = ManaPoolManager.Instance;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains("Shot"))
            {
                gameObject.GetComponent<SpriteRenderer>().size += new Vector2(0.03f, 0.03f);
                StartCoroutine(nameof(SetInactive));
                SpawnMana();                            
            }
        }

        #endregion

        #region Protected Virtual Functions

        //spawns the mana in the respective container
        protected virtual void SpawnMana()
        {
            Vector2 _spawnPointPosition = SpawnArray[(int)SpawnManaType].transform.position;
            Mana OrbMana = ManaTypes[(int)SpawnManaType];
            Debug.Log(SpawnManaType.ToString());
            Debug.Log("---------------");
      
            for (int i = 0; i < ManaAmount; i++)
            {
                float _spawnRandomiserX = Random.Range(-0.7f, 0.7f);
                float _spawnRandomiserY = Random.Range(-0.2f, 0.2f);

                Vector2 _spawnPosition = new(_spawnPointPosition.x + _spawnRandomiserX, _spawnPointPosition.y + _spawnRandomiserY);

                Mana tempMana = Instantiate(OrbMana, _spawnPosition, Quaternion.identity);

                switch (SpawnManaType)
                {
                    case ManaType.BaseMana:
                        _manaPoolManager.BasicMana.Add(tempMana);
                        break;

                    case ManaType.FireMana:
                        _manaPoolManager.FireMana.Add(tempMana);
                        break;

                    case ManaType.IceMana:
                        _manaPoolManager.IceMana.Add(tempMana);
                        break;

                    case ManaType.LightningMana:
                        _manaPoolManager.LightningMana.Add(tempMana);
                        break;

                    case ManaType.DarkMana:
                        _manaPoolManager.DarkMana.Add(tempMana);
                        break;

                    case ManaType.LightMana:
                        _manaPoolManager.LightMana.Add(tempMana);
                        break;
                }
            }
        }

        #endregion

        #region IEnumerators

        //Delays the "despawn" so that the size increase can be visible
        private IEnumerator SetInactive()
        {
            yield return new WaitForSeconds(0.1f);
            gameObject.GetComponent<SpriteRenderer>().size -= new Vector2(0.02f, 0.02f);
            gameObject.SetActive(false);
        }

        #endregion

    }
}

