using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using PeggleMana;
using PeggleWars.Audio;

namespace PeggleOrbs
{
    public class Orb : MonoBehaviour
    {
        #region Fields

        [SerializeField] protected Mana _orbMana;
        [SerializeField] protected ManaType SpawnManaType;
        [SerializeField] protected int ManaAmount = 10;
        protected GameObject[] SpawnArray;

        protected ManaPoolManager _manaPoolManager;

        protected Vector3 _position;

        #endregion

        #region Properties

        protected bool _isOccupied;

        public bool IsOccupied
        {
            get { return _isOccupied; }
            set { _isOccupied = value; }
        }

        [SerializeField] protected OrbType _orbType;

        public OrbType OrbType
        {
            get { return _orbType; }
            private set { _orbType = value; }
        }

        #endregion

        #region Public Functions





        #endregion

        #region Protected Virtual Functions

        protected virtual void Start()
        {
            //Ignore the collisions between layer 0 (default) and layer 8 (custom layer you set in Inspector window)
            Physics.IgnoreLayerCollision(6, 7);

            SpawnArray = GameObject.FindGameObjectsWithTag("ManaSpawn");
            _manaPoolManager = ManaPoolManager.Instance;
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            AudioManager.Instance.PlaySoundEffectNoLimit(SFX.BasicPeggleHit);
            gameObject.GetComponent<SpriteRenderer>().size += new Vector2(0.03f, 0.03f);
            OrbEffect();
            StartCoroutine(nameof(SetInactive));                         
        }

        protected virtual void OrbEffect()
        {
            SpawnMana();
        }

        //spawns the mana in the respective container
        protected virtual void SpawnMana()
        {
            Vector2 _spawnPointPosition = SpawnArray[(int)SpawnManaType].transform.position;
      
            for (int i = 0; i < ManaAmount; i++)
            {
                float _spawnRandomiserX = Random.Range(-0.7f, 0.7f);
                float _spawnRandomiserY = Random.Range(-0.2f, 0.2f);

                Vector2 _spawnPosition = new(_spawnPointPosition.x + _spawnRandomiserX, _spawnPointPosition.y + _spawnRandomiserY);

                Mana tempMana = Instantiate(_orbMana, _spawnPosition, Quaternion.identity);

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

        protected virtual void Awake()
        {
            _position = transform.position;
        }

        #endregion

        #region IEnumerators

        //Delays the "despawn" so that the size increase can be visible
        public IEnumerator SetInactive()
        {
            yield return new WaitForSeconds(0.1f);
            gameObject.GetComponent<SpriteRenderer>().size -= new Vector2(0.02f, 0.02f);
            gameObject.SetActive(false);
        }

        #endregion

    }
}

