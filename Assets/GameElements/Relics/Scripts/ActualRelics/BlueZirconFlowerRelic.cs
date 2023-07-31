 using EnumCollection;
using Orbs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace Relics
{
    internal class BlueZirconFlowerRelic : MonoBehaviour, IRelic
    {
        #region Fields and Properties

        internal static BlueZirconFlowerRelic Instance { get; private set; }
        public Relic RelicEnum { get; private set; } = Relic.BlueZirconFlower;

        public string Description { get; } = "<size=120%><b>Blue Zircon Flower</b><size=20%>\n\n<size=100%>" +
            "This light blue jewel cut in the shape " +
            "of an intricate flower feels cold in your hand.<size=20%>\n\n<size=100%>" +
            "Start each level with additional <b>1 Ice Mana</b>.";

        private readonly int _additionalIceMana = 10;

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

            OrbEvents.RaiseSpawnMana(ManaType.IceMana, _additionalIceMana);
        }

        #endregion
    }
}
