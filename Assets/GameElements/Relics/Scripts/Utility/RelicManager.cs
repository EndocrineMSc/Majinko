using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Relics
{
    internal class RelicManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static RelicManager Instance { get; private set; }

        [SerializeField] private RelicCollection _relicCollection;

        private List<Relic> _activeRelics;
        private Dictionary<Relic, GameObject> _allRelics;

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

            _activeRelics ??= new();
            _allRelics = _relicCollection.AllRelics;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name.Contains("Level") 
                || scene.name.Contains("World")
                || scene.name.Contains("Event"))
            {
                InstantiateRelics();
            }
        }

        private void InstantiateRelics()
        {
            foreach (var relic in _activeRelics)
                if (_activeRelics.Contains(relic))
                    Instantiate(_allRelics[relic], Vector3.zero, Quaternion.identity);
        }

        internal void AddRelic(Relic relic)
        {
            _activeRelics.Add(relic);
        }

        #endregion

    }

    internal enum Relic
    {
        BlueZirconFlower,
        ChillborneRing,
        GarnetFlower,
        IoliteFlower,
        SearingRune,
    }
}
