using Characters;
using Characters.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace Relics
{
    internal class SacredHeartwoodRelic : MonoBehaviour, IRelic
    {
        #region Fields and Properties

        internal static SacredHeartwoodRelic Instance { get; private set; }
        public Relic RelicEnum { get; private set; } = Relic.SacredHeartwood;

        public string Description => "<size=100%>Sacred Heartwood<size=90%>\r\n\r\n" +
            "This heart of a wooden, magical creature pulses with a dim, green light.\r\n\r\n" +
            "Heal 3 hp after each level.";

        private readonly int _healAmount = 3;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            UtilityEvents.OnLevelVictory += HealPlayer;
        }

        private void OnDisable()
        {
            UtilityEvents.OnLevelVictory -= HealPlayer;
        }

        private void Start()
        { 
            Image image = GetComponent<Image>();
            image.enabled = (SceneManager.GetActiveScene().name.Contains("Combat"));

            ScrollDisplayer displayer = GetComponent<ScrollDisplayer>();
            displayer.DisplayDescription = Description;
            displayer.DisplayScale = ScrollDisplayScales.RelicDisplayScale;
        }

        private void HealPlayer()
        {
            PlayerConditionTracker.HealPlayer(_healAmount);
            
            if (Player.Instance != null)
                Player.Instance.GetComponent<PopUpSpawner>().SpawnPopUp(_healAmount, "#0B6612"); //green color
        }
        #endregion
    }
}
