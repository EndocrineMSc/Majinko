using EnumCollection;
using Orbs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace Relics
{
    internal class GarnetFlowerRelic : MonoBehaviour, IRelic
    {
        #region Fields and Properties

        internal static GarnetFlowerRelic Instance { get; private set; }
        public Relic RelicEnum { get; private set; } = Relic.GarnetFlower;
        private readonly int _additionalFireMana = 10;

        //For displaying on scroll
        [SerializeField, TextArea] private string _displayDescription;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            Image image = GetComponent<Image>();
            image.enabled = (SceneManager.GetActiveScene().name.Contains("Level"));

            ScrollDisplayer displayer = GetComponent<ScrollDisplayer>();
            displayer.DisplayDescription = _displayDescription;
            displayer.DisplayScale = ScrollDisplayScales.RelicDisplayScale;

            OrbEvents.RaiseSpawnMana(ManaType.FireMana, _additionalFireMana);
        }

        #endregion
    }
}
