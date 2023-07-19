using EnumCollection;
using Orbs;
using UnityEngine;

namespace Relics
{
    internal class GarnetFlowerRelic : MonoBehaviour, IRelic
    {
        #region Fields and Properties

        internal static GarnetFlowerRelic Instance { get; private set; }
        public Relic RelicEnum { get; private set; } = Relic.GarnetFlower;
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
            OrbEvents.RaiseSpawnMana(ManaType.FireMana, _additionalFireMana);
        }

        #endregion
    }
}
