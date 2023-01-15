using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnumCollection;

namespace PeggleWars.Orbs
{
    public class OrbManager : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private List<Orb> _sceneOrbList = new();
        public static OrbManager Instance { get; private set; }
        public List<Orb> SceneOrbList { get => _sceneOrbList; set => _sceneOrbList = value; }

        //List of all orb prefabs, made in start() in alphabetical order from resources
        private List<Orb> _allOrbsList = new();

        #endregion

        #region Public Functions

        /// <summary>
        /// Will change a given amount of orbs into new ones, prioritizing switching out active BaseManaOrbs first.
        /// If not enough active BaseManaOrbs are present, it switches the active ones and then inactive ones.
        /// If no BaseManaOrbs are present, it switches random other active orbs first, then inactive random orbs second.
        /// </summary>
        /// <param name="orbType">Type of orb that will be instantiated into the level</param>
        /// <param name="switchAmount">Amount of orbs to be switched out</param>
        public void SwitchOrbs(OrbType orbType, int switchAmount)
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

        public void SetAllOrbsActive()
        {
            foreach (Orb orb in SceneOrbList)
            {
                orb.gameObject.SetActive(true);
            }
        }

        public void CheckForRefreshOrbs()
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
                //if working correctly, this should only trigger when no RefreshOrbs are present
                SwitchOrbs(OrbType.RefreshOrb, 1);
            }
        }

        #endregion

        #region Private Functions
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
            SceneOrbList = GameObject.FindObjectsOfType<Orb>().ToList();
            _allOrbsList = Resources.LoadAll<Orb>("OrbPrefabs").ToList();
        }

        private Orb FindRandomOrbInList(List<Orb> orbs)
        {
            int randomOrbIndex = Random.Range(0, orbs.Count - 1);

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
