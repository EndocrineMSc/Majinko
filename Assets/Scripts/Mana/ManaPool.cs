using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using PeggleWars.Orbs;
using System.Runtime.InteropServices.WindowsRuntime;

namespace PeggleWars.ManaManagement
{

    internal class ManaPool : MonoBehaviour
    {
        #region Fields and Properties

        internal List<Mana> BasicMana = new();
        internal List<Mana> FireMana = new();
        internal List<Mana> IceMana = new();

        private GameObject _baseManaSpawn;
        private GameObject _fireManaSpawn;
        private GameObject _iceManaSpawn;

        [SerializeField] private GameObject _basicManaPrefab;
        [SerializeField] private GameObject _fireManaPrefab;
        [SerializeField] private GameObject _iceManaPrefab;

        private readonly string BASEMANASPAWN_PARAM = "BaseManaSpawn";
        private readonly string FIREMANASPAWN_PARAM = "FireManaSpawn";
        private readonly string ICEMANASPAWN_PARAM = "IceManaSpawn";

        internal static ManaPool Instance { get; private set; }
        private OrbManager _orbManager;

        private int _manaCostMultiplier = 10;
        internal int ManaCostMultiplier { get => _manaCostMultiplier; }

        #endregion

        #region Functions

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
        }

        private void Start()
        {
            FindSpawnPoints();
            SetReferences();
            _orbManager.ManaSpawnTrigger?.Invoke(ManaType.BasicMana, 30);
        }

        private void FindSpawnPoints()
        {
            _baseManaSpawn = GameObject.FindGameObjectWithTag(BASEMANASPAWN_PARAM);
            _iceManaSpawn = GameObject.FindGameObjectWithTag(ICEMANASPAWN_PARAM);
            _fireManaSpawn = GameObject.FindGameObjectWithTag(FIREMANASPAWN_PARAM);
        }

        private void SetReferences()
        {
            _orbManager = OrbManager.Instance;
            _orbManager.ManaSpawnTrigger?.AddListener(SpawnMana);
        }

        internal void SpawnMana(ManaType manaType, int amount)
        {
            var spawnPointPosition = manaType switch
            {
                ManaType.BasicMana => _baseManaSpawn.transform.position,
                ManaType.FireMana => _iceManaSpawn.transform.position,
                ManaType.IceMana => _fireManaSpawn.transform.position,
                _ => _baseManaSpawn.transform.position,
            };
            for (int i = 0; i < amount; i++)
            {
                float _spawnRandomiserX = Random.Range(-0.2f, 0.2f);
                float _spawnRandomiserY = Random.Range(-0.2f, 0.2f);

                Vector2 _spawnPosition = new(spawnPointPosition.x + _spawnRandomiserX, spawnPointPosition.y + _spawnRandomiserY);
                GameObject tempManaObject;
                Mana tempMana;

                switch (manaType)
                {
                    case ManaType.BasicMana:
                        tempManaObject = Instantiate(_basicManaPrefab, _spawnPosition, Quaternion.identity);
                        tempMana = tempManaObject.GetComponent<Mana>();
                        BasicMana.Add(tempMana);
                        break;

                    case ManaType.FireMana:
                        tempManaObject = Instantiate(_iceManaPrefab, _spawnPosition, Quaternion.identity);
                        tempMana = tempManaObject.GetComponent<Mana>();
                        FireMana.Add(tempMana);
                        break;

                    case ManaType.IceMana:
                        tempManaObject = Instantiate(_fireManaPrefab, _spawnPosition, Quaternion.identity);
                        tempMana = tempManaObject.GetComponent<Mana>();
                        IceMana.Add(tempMana);
                        break;
                }
            }
        }

        internal void SpendMana(int basicManaAmount, int fireManaAmount, int iceManaAmount)
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



