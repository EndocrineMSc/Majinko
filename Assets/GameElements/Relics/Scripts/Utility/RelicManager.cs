using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace Relics
{
    internal class RelicManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static RelicManager Instance { get; private set; }

        [SerializeField] private RelicCollection _relicCollection;

        internal List<Relic> ActiveRelics { get; private set; }
        private List<Relic> _instantiatedRelics;
        private List<GameObject> _instantiatedRelicObjects;
        internal Dictionary<Relic, GameObject> AllRelics { get; private set; }

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
            AllRelics = _relicCollection.AllRelics;
            _relicLayoutGroup = transform.GetComponentInChildren<GridLayoutGroup>();

            ActiveRelics ??= ES3.KeyExists(RELIC_SAVE_PATH) ? ES3.Load<List<Relic>>(RELIC_SAVE_PATH) : new();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            UtilityEvents.OnGameReset += OnGameReset;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            UtilityEvents.OnGameReset -= OnGameReset;
        }

        private void OnApplicationQuit()
        {
            ES3.Save<List<Relic>>(RELIC_SAVE_PATH, ActiveRelics);
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
            foreach (var relic in ActiveRelics)
            {
                if (AllRelics[relic] != null && !_instantiatedRelics.Contains(relic))
                {
                    var relicObject = Instantiate(AllRelics[relic], Vector3.zero, Quaternion.identity);
                    relicObject.transform.SetParent(_relicLayoutGroup.transform);
                    _instantiatedRelics.Add(relic);
                    _instantiatedRelicObjects.Add(relicObject);
                }
            }
        }

        internal void AddRelic(Relic relic)
        {
            if (AllRelics.ContainsKey(relic))
                ActiveRelics.Add(relic);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            for (int i = 0; i < _instantiatedRelicObjects.Count; i++)
                if (_instantiatedRelicObjects[i] != null)
                    Destroy(_instantiatedRelicObjects[i]);

            _instantiatedRelics.Clear();
            _instantiatedRelicObjects.Clear();
        }

        private void OnGameReset()
        {
            if (ES3.KeyExists(RELIC_SAVE_PATH))
                ES3.DeleteKey(RELIC_SAVE_PATH);

            ActiveRelics.Clear();
        }

        #endregion
    }

    internal enum Relic
    {
        None, //null replacement for shop functions
        BlueZirconFlower,
        ChillborneRing,
        GarnetFlower,
        IoliteFlower,
        SacredHeartwood,
        SearingRune,
    }
}
