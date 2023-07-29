using UnityEngine;
using Orbs;
using EnumCollection;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace Relics
{
    internal class IoliteFlowerRelic : MonoBehaviour, IRelic
    {
        #region Fields and Properties

        internal static IoliteFlowerRelic Instance { get; private set; }
        public Relic RelicEnum { get; private set; } = Relic.IoliteFlower;

        public string Description => "<size=100%>Iolite Flower<size=90%>\r\n\r\n" +
            "This dark blue jewel cut in the shape of an intricate flower sparkles mysteriously" +
            " in your hand.\r\n\r\n" +
            "Start each level with an additional basic mana.";

        private readonly int _additionalBaseMana = 10;

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
            image.enabled = (SceneManager.GetActiveScene().name.Contains("Combat"));

            ScrollDisplayer displayer = GetComponent<ScrollDisplayer>();
            displayer.DisplayDescription = _displayDescription;
            displayer.DisplayScale = ScrollDisplayScales.RelicDisplayScale;

            OrbEvents.RaiseSpawnMana(ManaType.BasicMana, _additionalBaseMana);
        }

        #endregion
    }
}