using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System.Linq;
using System;

namespace PeggleWars.Orbs
{
    public class GlobalOrbManager : MonoBehaviour
    {

        #region Fields and Properties

        public static GlobalOrbManager Instance { get; private set; }

        [SerializeField] private List<Orb> _levelLoadOrbs = new();

        public List<Orb> LevelLoadOrbs
        {
            get { return _levelLoadOrbs; }
            private set { _levelLoadOrbs = value;}
        }

        [SerializeField] private List<Orb> _allOrbsList = new();

        public List<Orb> AllOrbsList
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
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            _allOrbsList = Resources.LoadAll<Orb>(ORBFOLDER_PARAM).ToList();
            _levelLoadOrbs.Add(_allOrbsList[(int)OrbType.RefreshOrb]);
            _levelLoadOrbs.Add(_allOrbsList[(int)(OrbType.FireManaOrb)]);
            _levelLoadOrbs.Add(_allOrbsList[(int)(OrbType.IceManaOrb)]);
        }

        public void AddGlobalOrb(Orb orb, int amount = 1)
        {           
            for (int i = 0; i < amount; i++)
            {
                _levelLoadOrbs.Add(orb);
            }
        }

        public void RemoveGlobalOrb(Orb orb)
        {
            _levelLoadOrbs.Remove(orb);
        }

        #endregion


    }
}
