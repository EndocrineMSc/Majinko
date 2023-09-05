using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using Orbs;
using System.Collections;
using Characters;

namespace ManaManagement
{
    public class ManaPool : MonoBehaviour
    {
        #region Fields and Properties

        public List<Mana> BasicMana = new();
        public List<Mana> FireMana = new();
        public List<Mana> IceMana = new();

        private GameObject _baseManaSpawn;
        private GameObject _fireManaSpawn;
        private GameObject _iceManaSpawn;

        [SerializeField] private GameObject _basicManaPrefab;
        [SerializeField] private GameObject _fireManaPrefab;
        [SerializeField] private GameObject _iceManaPrefab;
        [SerializeField] private GameObject _rottedManaPrefab;

        private readonly string BASEMANASPAWN_PARAM = "BaseManaSpawn";
        private readonly string FIREMANASPAWN_PARAM = "FireManaSpawn";
        private readonly string ICEMANASPAWN_PARAM = "IceManaSpawn";

        public static ManaPool Instance { get; private set; }

        private readonly int _manaCostMultiplier = 10;
        public int ManaCostMultiplier { get => _manaCostMultiplier; }

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            FindSpawnPoints();
            OrbEvents.SpawnMana += SpawnManaWrap;
            OrbEvents.RaiseSpawnMana(ManaType.BasicMana, 30);

            if (PlayerConditionTracker.TemporaryBasicMana > 0)
            {
                OrbEvents.RaiseSpawnMana(ManaType.BasicMana, PlayerConditionTracker.TemporaryBasicMana * ManaCostMultiplier);
                PlayerConditionTracker.ResetTemporaryBasicMana();
            }
        }

        private void OnDisable()
        {
            OrbEvents.SpawnMana -= SpawnManaWrap;
        }

        private void FindSpawnPoints()
        {
            _baseManaSpawn = GameObject.FindGameObjectWithTag(BASEMANASPAWN_PARAM);
            _iceManaSpawn = GameObject.FindGameObjectWithTag(ICEMANASPAWN_PARAM);
            _fireManaSpawn = GameObject.FindGameObjectWithTag(FIREMANASPAWN_PARAM);
        }

        private void SpawnManaWrap(ManaType manaType, int amount)
        {
            StartCoroutine(SpawnMana(manaType, amount));
        }

        private IEnumerator SpawnMana(ManaType manaType, int amount)
        {
            var spawnPointPosition = manaType switch
            {
                ManaType.BasicMana => _baseManaSpawn.transform.position,
                ManaType.FireMana => _fireManaSpawn.transform.position,
                ManaType.IceMana => _iceManaSpawn.transform.position,
                ManaType.RottedMana => _baseManaSpawn.transform.position, //intended as base mana spawn, no typo
                _ => _baseManaSpawn.transform.position,
            };
            for (int i = 0; i < amount; i++)
            {
                float _spawnRandomiserX = Random.Range(-0.2f, 0.2f);
                float _spawnRandomiserY = Random.Range(-0.2f, 0.2f);

                Vector2 _spawnPosition = new((spawnPointPosition.x + _spawnRandomiserX), (spawnPointPosition.y + _spawnRandomiserY));
                GameObject tempManaObject;
                Mana tempMana;

                switch (manaType)
                {
                    case ManaType.BasicMana:
                        tempManaObject = Instantiate(_basicManaPrefab, _spawnPosition, Quaternion.identity);
                        tempMana = tempManaObject.GetComponent<Mana>();
                        BasicMana.Add(tempMana);
                        yield return null;
                        break;

                    case ManaType.FireMana:
                        tempManaObject = Instantiate(_fireManaPrefab, _spawnPosition, Quaternion.identity);
                        tempMana = tempManaObject.GetComponent<Mana>();
                        FireMana.Add(tempMana);
                        yield return new WaitForSeconds(0.5f);
                        break;

                    case ManaType.IceMana:
                        tempManaObject = Instantiate(_iceManaPrefab, _spawnPosition, Quaternion.identity);
                        tempMana = tempManaObject.GetComponent<Mana>();
                        IceMana.Add(tempMana);
                        yield return new WaitForSeconds(0.75f);
                        break;

                    case ManaType.RottedMana:
                        tempManaObject = Instantiate(_rottedManaPrefab, _spawnPosition, Quaternion.identity);
                        tempMana = tempManaObject.GetComponent<Mana>();
                        BasicMana.Add(tempMana);
                        yield return new WaitForSeconds(0.75f);
                        break;
                }
            }
        }

        public void SpendMana(int basicManaAmount, int fireManaAmount, int iceManaAmount)
        {
            SpendManaByList(BasicMana, basicManaAmount);
            SpendManaByList(FireMana, fireManaAmount);
            SpendManaByList(IceMana, iceManaAmount);
        }       

        private void SpendManaByList(List<Mana> manaList, int amount)
        {
            if (manaList.Count < amount)
            {
                Debug.Log("Not enough Mana");
            }
            else
            {
                for (int i = 0; i < amount; i++)
                {
                    Destroy(manaList[0].gameObject);
                    manaList.RemoveAt(0);
                }
            }
        }

        #endregion
    }
}



