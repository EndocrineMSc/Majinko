using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System.Linq;

namespace PeggleWars.Orbs
{
    internal class GlobalOrbManager : MonoBehaviour
    {

        #region Fields and Properties

        internal static GlobalOrbManager Instance { get; private set; }

        [SerializeField] private List<Orb> _levelLoadOrbs = new();

        internal List<Orb> LevelLoadOrbs
        {
            get { return _levelLoadOrbs; }
            private set { _levelLoadOrbs = value;}
        }

        [SerializeField] private List<Orb> _allOrbsList = new();

        internal List<Orb> AllOrbsList
        {
            get { return _allOrbsList; }
            private set { _allOrbsList = value;}
        }

        private string ORBFOLDER_PARAM = "OrbPrefabs";

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            _allOrbsList = Resources.LoadAll<Orb>(ORBFOLDER_PARAM).ToList();
            _levelLoadOrbs.Add(_allOrbsList[(int)OrbType.RefreshOrb]);
            _levelLoadOrbs.Add(_allOrbsList[(int)(OrbType.FireManaOrb)]);
            _levelLoadOrbs.Add(_allOrbsList[(int)(OrbType.IceManaOrb)]);
            _levelLoadOrbs.Add(_allOrbsList[(int)(OrbType.LightningStrikeOrb)]);
        }

        internal void AddGlobalOrb(Orb orb, int amount = 1)
        {           
            for (int i = 0; i < amount; i++)
            {
                _levelLoadOrbs.Add(orb);
            }
        }

        internal void RemoveGlobalOrb(Orb orb)
        {
            _levelLoadOrbs.Remove(orb);
        }

        #endregion


    }
}
