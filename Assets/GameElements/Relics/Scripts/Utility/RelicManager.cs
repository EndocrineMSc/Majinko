using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Relics
{
    internal class RelicManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static RelicManager Instance { get; private set; }

        [SerializeField] private RelicCollection _relicCollection;

        private List<Relic> _activeRelics;
        private List<Relic> _instantiatedRelics;
        private List<GameObject> _instantiatedRelicObjects;
        private Dictionary<Relic, GameObject> _allRelics;

        private GridLayoutGroup _relicLayoutGroup;
        private readonly string RELIC_SAVE_PATH = "ActiveRelics";

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

            _instantiatedRelics ??= new();
            _instantiatedRelicObjects ??= new();
            _allRelics = _relicCollection.AllRelics;
            _relicLayoutGroup = transform.GetComponentInChildren<GridLayoutGroup>();

            _activeRelics ??= ES3.KeyExists(RELIC_SAVE_PATH) ? ES3.Load<List<Relic>>(RELIC_SAVE_PATH) : new();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnApplicationQuit()
        {
            ES3.Save<List<Relic>>(RELIC_SAVE_PATH, _activeRelics);
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

        internal void InstantiateRelics()
        {
            foreach (var relic in _activeRelics)
            {
                if (_allRelics[relic] != null && !_instantiatedRelics.Contains(relic))
                {
                    var relicObject = Instantiate(_allRelics[relic], Vector3.zero, Quaternion.identity);
                    relicObject.transform.SetParent(_relicLayoutGroup.transform);
                    _instantiatedRelics.Add(relic);
                    _instantiatedRelicObjects.Add(relicObject);
                }
            }
        }

        internal void AddRelic(Relic relic)
        {
            if (_allRelics.ContainsKey(relic))
                _activeRelics.Add(relic);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            for (int i = 0; i < _instantiatedRelicObjects.Count; i++)
                if (_instantiatedRelicObjects[i] != null)
                    Destroy(_instantiatedRelicObjects[i]);

            _instantiatedRelics.Clear();
            _instantiatedRelicObjects.Clear();
        }

        #endregion
    }

    internal enum Relic
    {
        BlueZirconFlower,
        ChillborneRing,
        GarnetFlower,
        IoliteFlower,
        SacredHeartwood,
        SearingRune,
    }
}
