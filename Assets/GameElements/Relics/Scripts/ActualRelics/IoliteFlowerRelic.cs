using UnityEngine;
using Orbs;
using EnumCollection;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Relics
{
    internal class IoliteFlowerRelic : MonoBehaviour, IRelic
    {
        #region Fields and Properties

        internal static IoliteFlowerRelic Instance { get; private set; }
        public Relic RelicEnum { get; private set; } = Relic.IoliteFlower;
        private readonly int _additionalBaseMana = 10;

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

            OrbEvents.RaiseSpawnMana(ManaType.BasicMana, _additionalBaseMana);
        }

        #endregion
    }
}
