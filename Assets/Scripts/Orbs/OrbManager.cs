using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnumCollection;
using System;
using PeggleWars.ManaManagement;

namespace PeggleWars.Orbs
{
    [Serializable]
    internal class OrbManager : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private List<Orb> _sceneOrbList = new();
        internal static OrbManager Instance { get; private set; }
        internal List<Orb> SceneOrbList { get => _sceneOrbList; set => _sceneOrbList = value; }

        //List of all orb prefabs, made in start() in alphabetical order from resources
        private List<Orb> _allOrbsList = new();

        [SerializeField] private ScriptableOrbLayout _layout;

        internal ManaSpawnEvent ManaSpawnTrigger;
        
        private GlobalOrbManager _globalOrbManager;

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
            }
        }

        private void Start()
        {
            _globalOrbManager = GlobalOrbManager.Instance;
            
            ManaSpawnTrigger = new ManaSpawnEvent();

            OrbGridPositions test = new();
            _allOrbsList = _globalOrbManager.AllOrbsList;

            bool[,] orbLayout = _layout.InitializeOrbLayout();

            for (int x = 0; x < orbLayout.GetLength(0); x++)
            {
                for (int y = 0; y < orbLayout.GetLength(1); y++)
                {
                    if (orbLayout[x, y])
                    {
                        Vector2 instantiatePosition = test.GridArray[x, y];
                        Instantiate(_allOrbsList[0], instantiatePosition, Quaternion.identity);
                    }
                    
                }
            }
            
            SceneOrbList = GameObject.FindObjectsOfType<Orb>().ToList();
            InsertLevelLoadOrbs();
        }

        private void InsertLevelLoadOrbs()
        {
            foreach (Orb orb in _globalOrbManager.LevelLoadOrbs)
            {
                SwitchOrbs(orb.OrbType, 1);
            }
        }

        internal void SwitchOrbs(OrbType orbType, int switchAmount = 1)
        {
            List<Orb> baseOrbs = FindOrbs(SceneOrbList, SearchTag.BaseOrbs);          
            List<Orb> activeBaseOrbs = FindOrbs(baseOrbs, SearchTag.IsActive);
           
            Orb orbToBeInserted = _allOrbsList[(int)orbType];

            //Case 1: enough active base orbs present, so switch some of those
            if (activeBaseOrbs.Count >= switchAmount)
            {
                ReplaceOrbsInList(activeBaseOrbs, switchAmount, orbToBeInserted);
            }
            //Case 2: enough base orbs are present, but some of them are inactive, so switch the active ones first, then the inactive ones
            else if (baseOrbs.Count >= switchAmount)
            {
                List<Orb> inactiveBaseOrbs = FindOrbs(baseOrbs, SearchTag.IsInactive);

                int availableOrbs = activeBaseOrbs.Count;
                ReplaceOrbsInList(activeBaseOrbs, availableOrbs, orbToBeInserted);

                int missingOrbs = switchAmount - activeBaseOrbs.Count;
                ReplaceOrbsInList(inactiveBaseOrbs, missingOrbs, orbToBeInserted);
            }
            //Case 3: there are only non-base orbs left (unlikely) so switch active ones first then inactive ones second
            else
            {
                List<Orb> activeOrbs = FindOrbs(SceneOrbList, SearchTag.IsActive);
                List<Orb> inactiveOrbs = FindOrbs(SceneOrbList, SearchTag.IsInactive);

                int availableOrbs = activeOrbs.Count;
                ReplaceOrbsInList(activeOrbs, availableOrbs, orbToBeInserted);

                int missingOrbs = switchAmount - activeOrbs.Count;
                ReplaceOrbsInList(inactiveOrbs, missingOrbs, orbToBeInserted);
            }
        }

        internal void SetAllOrbsActive()
        {
            foreach (Orb orb in SceneOrbList)
            {
                orb.gameObject.SetActive(true);
            }
        }

        internal void CheckForRefreshOrbs()
        {
            bool refreshPresent = false;

            foreach (Orb orb in SceneOrbList)
            {
                if (orb.OrbType == OrbType.RefreshOrb)
                {
                    refreshPresent = true;
                }
            }

            if (!refreshPresent)
            {
                SwitchOrbs(OrbType.RefreshOrb, 1);
            }
        }

        private Orb FindRandomOrbInList(List<Orb> orbs)
        {
            int randomOrbIndex = UnityEngine.Random.Range(0, orbs.Count - 1);

            Orb randomOrb = orbs[randomOrbIndex];

            return randomOrb;
        }

        private List<Orb> FindOrbs(List<Orb> orbs, SearchTag searchTag)
        {
            List<Orb> resultOrbs = new();

            foreach (Orb tempOrb in orbs)
            {
                switch (searchTag)
                {
                    case SearchTag.BaseOrbs:
                        if (tempOrb.OrbType == OrbType.BaseManaOrb)
                        {
                            resultOrbs.Add(tempOrb);
                        }
                        break;

                    case SearchTag.IsActive:
                        if (tempOrb.isActiveAndEnabled)
                        {
                            resultOrbs.Add(tempOrb);
                        }
                        break;

                    case SearchTag.IsInactive:
                        if (!tempOrb.gameObject.activeSelf) 
                        { 
                            resultOrbs.Add(tempOrb);
                        }
                        break;
                }
            }
            return resultOrbs;
        }
        
        private void ReplaceOrbsInList(List<Orb> orbs, int amount, Orb orb)
        {
            for (int i = 0; i < amount; i++)
            {
                Orb randomOrb = FindRandomOrbInList(orbs);
                Vector3 randomOrbPosition = randomOrb.transform.position;
                Orb tempOrb = Instantiate(orb, randomOrbPosition, Quaternion.identity);

                Instance.SceneOrbList.Remove(randomOrb);
                Instance.SceneOrbList.Add(tempOrb);

                Destroy(randomOrb.gameObject);                            
            }
        }
        #endregion

        private enum SearchTag
        {
            BaseOrbs,
            IsActive,
            IsInactive,
        }
    }
}
