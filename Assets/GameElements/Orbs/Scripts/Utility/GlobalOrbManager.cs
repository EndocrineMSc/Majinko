using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System.Linq;
using Utility;

namespace Orbs
{
    internal class GlobalOrbManager : MonoBehaviour, IResetOnQuit
    {

        #region Fields and Properties

        internal static GlobalOrbManager Instance { get; private set; }

        internal List<Orb> LevelLoadOrbs { get; private set; }
        internal List<Orb> AllOrbsList { get; private set; }
        internal int AmountOfRefreshOrbs { get; private set; } = 1;

        [SerializeField] private AllOrbsCollection _allOrbsCollection;

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
                Destroy(gameObject);

            InitializeManager();
        }

        private void OnEnable()
        {
            UtilityEvents.OnGameReset += OnGameReset;
        }

        private void OnDisable()
        {
            UtilityEvents.OnGameReset -= OnGameReset;
        }

        private void InitializeManager()
        {
            InitializeLists();
            AllOrbsList = _allOrbsCollection.AllOrbs.ToList();
            AllOrbsList = AllOrbsList.OrderBy(x => x.OrbType).ToList();
            LevelLoadOrbs.Add(AllOrbsList[(int)OrbType.RefreshOrb]);
            LevelLoadOrbs.Add(AllOrbsList[(int)OrbType.FireManaOrb]);
            LevelLoadOrbs.Add(AllOrbsList[(int)OrbType.IceManaOrb]);
        }

        private void InitializeLists()
        {
            AllOrbsList ??= new();
            LevelLoadOrbs ??= new();
        }

        internal void AddGlobalOrb(Orb orb, int amount = 1)
        {           
            for (int i = 0; i < amount; i++)
            {
                LevelLoadOrbs.Add(orb);

                if (orb.OrbType == OrbType.RefreshOrb)
                {
                    AmountOfRefreshOrbs++;
                }
            }
        }

        internal void RemoveGlobalOrb(Orb orb)
        {
            LevelLoadOrbs.Remove(orb);

            if (orb.OrbType == OrbType.RefreshOrb) {AmountOfRefreshOrbs--;}
        }

        public void OnGameReset()
        {
            LevelLoadOrbs.Clear();
            InitializeManager();
        }
        #endregion
    }
}
