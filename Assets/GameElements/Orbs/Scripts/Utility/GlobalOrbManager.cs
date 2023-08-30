using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System.Linq;
using Utility;

namespace Orbs
{
    internal class GlobalOrbManager : MonoBehaviour, IResetOnQuit
    {

        #region Fields and Properties

        internal static GlobalOrbManager Instance { get; private set; }

        internal List<Orb> LevelLoadOrbs { get; private set; }
        internal List<Orb> AllOrbsList { get; private set; }
        internal int AmountOfRefreshOrbs { get; private set; } = 0;

        [SerializeField] private AllOrbsCollection _allOrbsCollection;

        private readonly string LEVELLOADORBS_PATH = "LevelLoadOrbs";
        private readonly string REFRESHORBS_PATH = "RefreshOrbs";

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            InitializeManager();
        }

        private void OnEnable()
        {
            UtilityEvents.OnGameReset += OnGameReset;
        }

        private void OnDisable()
        {
            UtilityEvents.OnGameReset -= OnGameReset;
        }

        private void Start()
        {
            if (ES3.KeyExists(LEVELLOADORBS_PATH))
            {
                AmountOfRefreshOrbs = 0;
                LevelLoadOrbs.Clear();
                var tempList = ES3.Load(LEVELLOADORBS_PATH) as List<Orb>;
                foreach (Orb orb in tempList)
                    AddLevelLoadOrb(orb.OrbType);
            }
        }

        private void OnApplicationQuit()
        {
            ES3.Save(LEVELLOADORBS_PATH, LevelLoadOrbs);
            ES3.Save(REFRESHORBS_PATH, AmountOfRefreshOrbs);
        }

        private void InitializeManager()
        {
            InitializeLists();
            AllOrbsList = _allOrbsCollection.AllOrbs.ToList();
            AllOrbsList = AllOrbsList.OrderBy(x => x.OrbType).ToList();

            if (LevelLoadOrbs.Count == 0 && !ES3.KeyExists(LEVELLOADORBS_PATH))
            {
                AddLevelLoadOrb(OrbType.IceManaOrb);
                AddLevelLoadOrb(OrbType.FireManaOrb);
                AddLevelLoadOrb(OrbType.RefreshOrb);
            }
        }

        private void InitializeLists()
        {
            AllOrbsList ??= new();
            LevelLoadOrbs ??= new();
        }

        internal void AddLevelLoadOrb(OrbType orbType, int amount = 1)
        {           
            for (int i = 0; i < amount; i++)
            {
                LevelLoadOrbs.Add(AllOrbsList[(int)orbType]);

                if (orbType == OrbType.RefreshOrb)
                {
                    AmountOfRefreshOrbs++;
                }
            }
        }

        internal void RemoveGlobalOrb(Orb orb)
        {
            LevelLoadOrbs.Remove(orb);

            if (orb.OrbType == OrbType.RefreshOrb) 
                AmountOfRefreshOrbs--;
        }

        public void OnGameReset()
        {
            LevelLoadOrbs.Clear();
            AmountOfRefreshOrbs = 0;
            ES3.DeleteKey(LEVELLOADORBS_PATH);
            ES3.DeleteKey(REFRESHORBS_PATH);
            InitializeManager();
        }
        #endregion
    }
}
