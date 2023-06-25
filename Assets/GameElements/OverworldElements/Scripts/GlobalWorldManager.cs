using Overworld;
using UnityEngine;

namespace Utility
{
    internal class GlobalWorldManager : MonoBehaviour, IResetOnQuit
    {
        #region Fields and Properties

        internal static GlobalWorldManager Instance { get; private set; }
        internal int WorldIndex { get; private set; } = 1;

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

        internal void ChangeWorldIndex()
        {
            WorldIndex += 1;
        }

        public void OnGameReset()
        {
            WorldIndex = 1;
            CurrentPlayerWorldPosition.SetPlayerButtonIndex(0);
        }

        #endregion
    }
}