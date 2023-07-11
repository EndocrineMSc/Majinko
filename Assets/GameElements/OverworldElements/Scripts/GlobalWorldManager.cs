using Characters;
using Overworld;
using UnityEngine;

namespace Utility
{
    internal class GlobalWorldManager : MonoBehaviour, IResetOnQuit
    {
        #region Fields and Properties

        internal static GlobalWorldManager Instance { get; private set; }
        internal int WorldIndex { get; private set; } = 1;
        private readonly string WORLD_SAVE_PATH = "WorldIndex";
        private readonly string PLAYER_SAVE_PATH = "PlayerIndex";
        private readonly string MAXHEALTH_SAVE_PATH = "PlayerMaxHealth";
        private readonly string HEALTH_SAVE_PATH = "PlayerHealth";


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
            if (ES3.KeyExists(WORLD_SAVE_PATH))
                GlobalWorldManager.Instance.WorldIndex = ES3.Load<int>(WORLD_SAVE_PATH);

            if (ES3.KeyExists(PLAYER_SAVE_PATH))
                CurrentPlayerWorldPosition.SetPlayerButtonIndex(ES3.Load<int>(PLAYER_SAVE_PATH));

            if (ES3.KeyExists(HEALTH_SAVE_PATH))
                PlayerConditionTracker.SetPlayerHealth(ES3.Load<int>(HEALTH_SAVE_PATH));

            if (ES3.KeyExists(MAXHEALTH_SAVE_PATH))
                PlayerConditionTracker.SetMaxHealth(ES3.Load<int>(MAXHEALTH_SAVE_PATH));
        }

        private void OnApplicationQuit()
        {
            ES3.Save(WORLD_SAVE_PATH, WorldIndex);
            ES3.Save(PLAYER_SAVE_PATH, CurrentPlayerWorldPosition.OverworldPlayerButtonIndex);
            ES3.Save(MAXHEALTH_SAVE_PATH, PlayerConditionTracker.MaxPlayerHealth);
            ES3.Save(HEALTH_SAVE_PATH, PlayerConditionTracker.PlayerHealth);
        }

        internal void ChangeWorldIndex()
        {
            WorldIndex += 1;
        }

        public void OnGameReset()
        {
            WorldIndex = 1;
            CurrentPlayerWorldPosition.SetPlayerButtonIndex(0);
            PlayerConditionTracker.OnGameReset();
            ES3.DeleteKey(WORLD_SAVE_PATH);
            ES3.DeleteKey(PLAYER_SAVE_PATH);
        }

        #endregion
    }
}