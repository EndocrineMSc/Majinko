using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using PeggleWars.Orbs;

namespace PeggleWars.ManaManagement
{
    /// <summary>
    /// This class handles the the usage of mana. Stores available mana in lists.
    /// </summary>
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

        private string BASEMANASPAWN_PARAM = "BaseManaSpawn";
        private string FIREMANASPAWN_PARAM = "FireManaSpawn";
        private string ICEMANASPAWN_PARAM = "IceManaSpawn";

        public static ManaPool Instance { get; private set; }
        private OrbManager _orbManager;

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
            _orbManager.ManaSpawnTrigger?.Invoke(ManaType.BaseMana, 30);
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

        public void SpawnMana(ManaType manaType, int amount)
        {
            var spawnPointPosition = manaType switch
            {
                ManaType.BaseMana => (Vector2)_baseManaSpawn.transform.position,
                ManaType.FireMana => (Vector2)_iceManaSpawn.transform.position,
                ManaType.IceMana => (Vector2)_fireManaSpawn.transform.position,
                _ => (Vector2)_baseManaSpawn.transform.position,
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
                    case ManaType.BaseMana:
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

        public void SpendMana(ManaType manaType, int amount)
        {
            switch (manaType)
            {
                case ManaType.BaseMana:
                    SpendManaByList(BasicMana, amount);
                    break;

                case ManaType.FireMana:
                    SpendManaByList(FireMana, amount);
                    break;

                case ManaType.IceMana:
                    SpendManaByList(IceMana, amount);
                    break;
            }
        }

        public bool CheckForManaAmount(ManaType manaType, int amount)
        {
            bool enoughMana = false;

            switch (manaType)
            {
                case ManaType.BaseMana:
                    enoughMana = CheckIfEnoughManaByList(BasicMana, amount);
                    break;

                case ManaType.FireMana:
                    enoughMana = CheckIfEnoughManaByList(FireMana, amount);
                    break;

                case ManaType.IceMana:
                    enoughMana = CheckIfEnoughManaByList(IceMana, amount);
                    break;
            }
            return enoughMana;
        }

        private void SpendManaByList(List<Mana> manaList, int amount)
        {
            if (manaList.Count < amount)
            {
                Debug.Log("Not enough Mana");
                return;
            }

            for (int i = 0; i < amount; i++)
            { 
                Destroy(manaList[0].gameObject);
                manaList.RemoveAt(0);
            }   
        }

        private bool CheckIfEnoughManaByList(List<Mana> manaList, int amount)
        {
            if (manaList.Count > amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion
    }
}



