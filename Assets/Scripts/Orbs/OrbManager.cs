using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnumCollection;
using PeggleWars.TurnManagement;
using PeggleWars;

namespace PeggleOrbs
{
    public class OrbManager : MonoBehaviour
    {
        #region Fields/Properties

        [SerializeField] private List<Orb> _sceneOrbList = new();
        public static OrbManager Instance { get; private set; }
        public List<Orb> SceneOrbList { get => _sceneOrbList; set => _sceneOrbList = value; }

        //list of all orb prefabs, made in start() in alphabetical order from resources
        private List<Orb> _allOrbsList = new();

        #endregion

        #region Public Functions

        //Will change int amount of orbs into given orb
        //Will only do so for active orbs
        //If not enough active orbs are present, will activate
        //missing orbs, will prefer BaseManaOrbs to exchange first
        public void SwitchOrbs(OrbType orbType, int amount)
        {
            List<Orb> baseOrbs = FindOrbs(Instance.SceneOrbList, SearchTag.BaseOrbs);
            List<Orb> activeBaseOrbs;
            Orb orb = _allOrbsList[(int)orbType];

            //no BaseManaOrbs present, so replace any other active Orbs
            if (baseOrbs.Count == 0) 
            {
                List<Orb> activeOrbs = FindOrbs(Instance.SceneOrbList, SearchTag.IsActive);

                //enough other active orbs are present, so replace some of those
                if(activeOrbs.Count >= amount)
                {
                    ReplaceOrbsInList(activeOrbs, amount, orb);
                }
                //not enough active orbs present, so replace remaining active orbs, then take inactive ones
                else
                {
                    int availableOrbs = activeOrbs.Count;
                    ReplaceOrbsInList(activeOrbs, availableOrbs, orb);

                    int missingOrbs = amount - activeOrbs.Count;
                    List<Orb> inactiveOrbs = FindOrbs(_sceneOrbList, SearchTag.IsInactive);
                    ReplaceOrbsInList(inactiveOrbs, missingOrbs, orb);
                }
            }
            //are there enough remaining baseOrbs present, whether active or inactive, then replace active ones first
            else if (baseOrbs.Count >= amount)
            {
                activeBaseOrbs = FindOrbs(baseOrbs, SearchTag.IsActive);
                
                //all base orbs are inactive, so replace random ones in that list
                if (activeBaseOrbs.Count == 0)
                {
                    ReplaceOrbsInList(baseOrbs, amount, orb);

                }
                else if (activeBaseOrbs.Count >= amount)
                {
                    //best case: enough active, unoccupied baseorbs
                    ReplaceOrbsInList(activeBaseOrbs, amount, orb);
                }              
            }
            //if here, then there are baseOrbs but not enough for the whole amount
            else if (baseOrbs.Count > 0)
            {
                int availableOrbs = baseOrbs.Count;
                int missingOrbs = amount - availableOrbs;

                ReplaceOrbsInList(baseOrbs,availableOrbs, orb);

                List<Orb> activeOrbs = FindOrbs(Instance.SceneOrbList, SearchTag.IsActive);

                //enough other active orbs are present, so replace some of those
                if (activeOrbs.Count >= missingOrbs)
                {
                    ReplaceOrbsInList(activeOrbs, missingOrbs, orb);
                }
                //not enough active orbs present, so replace remaining active orbs, then take inactive ones
                else
                {
                    int availableActiveOrbs = activeOrbs.Count;
                    ReplaceOrbsInList(activeOrbs, availableActiveOrbs, orb);

                    int missingActiveOrbs = missingOrbs - activeOrbs.Count;
                    List<Orb> inactiveOrbs = FindOrbs(_sceneOrbList, SearchTag.IsInactive);
                    ReplaceOrbsInList(inactiveOrbs, missingActiveOrbs, orb);
                }
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

        // Start is called before the first frame update
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
