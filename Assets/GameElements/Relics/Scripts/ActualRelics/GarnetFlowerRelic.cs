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

        public string Description => "<size=100%>Garnet Flower<size=90%>\r\n\r\n" +
            "This red jewel cut in the shape of an intricate flower feels warm in your hand.\r\n\r\n" +
            "Start each level with an additional fire mana.\r\n";

        private readonly int _additionalFireMana = 10;

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
            displayer.DisplayDescription = Description;
            displayer.DisplayScale = ScrollDisplayScales.RelicDisplayScale;

            OrbEvents.RaiseSpawnMana(ManaType.FireMana, _additionalFireMana);
        }

        #endregion
    }
}
