using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System.Linq;
using Utility;

namespace Orbs
{
    public class GlobalOrbManager : MonoBehaviour, IResetOnQuit
    {

        #region Fields and Properties

        public static GlobalOrbManager Instance { get; private set; }

        public List<OrbData> LevelLoadOrbs { get; private set; } = new();
        public int AmountOfRefreshOrbs { get; private set; } = 0;

        [SerializeField] private OrbData[] _standardLevelLoadOrbData;

        private readonly string LEVELLOADORBS_PATH = "LevelLoadOrbs";
        private readonly string REFRESHORBS_PATH = "RefreshOrbs";
        private readonly string REFRESHORB_NAME = "Refresh";

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
            {
                Destroy(gameObject);
            }
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

                List<OrbData> loadedLevelLoadOrbs = ES3.Load(LEVELLOADORBS_PATH) as List<OrbData>;
                foreach (var orbData in loadedLevelLoadOrbs)
                    AddLevelLoadOrb(orbData);
            }
            InitializeManager();
        }

        private void OnApplicationQuit()
        {
            ES3.Save(LEVELLOADORBS_PATH, LevelLoadOrbs);
            ES3.Save(REFRESHORBS_PATH, AmountOfRefreshOrbs);
        }

        private void InitializeManager()
        {
            if (LevelLoadOrbs.Count == 0 && !ES3.KeyExists(LEVELLOADORBS_PATH))
            {
                foreach (var orbData in _standardLevelLoadOrbData)
                    AddLevelLoadOrb(orbData);

                AmountOfRefreshOrbs++;
            }
        }

        public void AddLevelLoadOrb(OrbData orbData, int amount = 1)
        {           
            for (int i = 0; i < amount; i++)
                LevelLoadOrbs.Add(orbData);
        }

        public void RemoveGlobalOrb(OrbData orbData)
        {
            //remove first corresponding orbData from list
            foreach (var orb in LevelLoadOrbs)
            {
                if (orb.OrbName == orbData.OrbName)
                {
                    LevelLoadOrbs.Remove(orb);
                    break;
                }
            }    

            //check for amount first, to avoid negative amounts in edge cases
            if (orbData.OrbName.Contains(REFRESHORB_NAME) && AmountOfRefreshOrbs > 0) 
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
