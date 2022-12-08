using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnumCollection;
using PeggleOrbs.TransientOrbs;

namespace PeggleOrbs
{
    public class OrbManager : MonoBehaviour
    {
        #region Fields

        private List<Orb> _orbList = new List<Orb>();
        public static OrbManager Instance;

        #endregion

        #region Public Functions

        //Will change int amount of orbs into given orb
        //Will only do so for active orbs
        //If not enough active orbs are present, will active
        //missing orbs, will prefer BaseManaOrbs to exchange first
        public void SwitchTransientOrb(TransientOrb orb, int amount)
        {
            List<Orb> baseOrbs = FindOrbs(_orbList, SearchTag.BaseOrbs);
            List<Orb> unoccupiedBaseOrbs = new();
            List<Orb> activeUnoccupiedBaseOrbs = new();
            
            //no BaseManaOrbs present, so replace any other active Orbs
            if (baseOrbs.Count == 0) 
            {
                List<Orb> activeOrbs = FindOrbs(_orbList, SearchTag.IsActive);

                if(activeOrbs.Count >= amount)
                {
                    ReplaceOrbsInList(activeOrbs, amount, orb, false);
                    return;
                }                
            }
            else
            {
                unoccupiedBaseOrbs = FindOrbs(baseOrbs, SearchTag.IsUnoccupied);
            }

            if (unoccupiedBaseOrbs.Count == 0)
            {
                //end shit right here
            }
            else
            {
                activeUnoccupiedBaseOrbs = FindOrbs(unoccupiedBaseOrbs, SearchTag.IsActive);
            }

            //best case: enough active, unoccupied baseorbs
            if (activeUnoccupiedBaseOrbs.Count >= amount)
            {
                ReplaceOrbsInList(activeUnoccupiedBaseOrbs, amount, orb, true);
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
            _orbList = GameObject.FindObjectsOfType<Orb>().ToList();
        }

        private Orb FindRandomOrbInList(List<Orb> orbs)
        {
            int randomOrbIndex = Random.Range(0, _orbList.Count - 1);

            Orb randomOrb = orbs[randomOrbIndex];

            return randomOrb;
        }

        private List<Orb> FindOrbs(List<Orb> orbs, SearchTag searchTag)
        {
            List<Orb> resultOrbs = new List<Orb>();

            foreach (Orb tempOrb in orbs)
            {
                switch (searchTag)
                {
                    case SearchTag.BaseOrbs:
                        if (tempOrb.OrbType == OrbType.BaseOrb)
                        {
                            resultOrbs.Add(tempOrb);
                        }
                        break;

                    case SearchTag.IsUnoccupied:
                        if (tempOrb.IsOccupied)
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
                }
            }
            return resultOrbs;
        }
        
        private void ReplaceOrbsInList(List<Orb> orbs, int amount, TransientOrb orb, bool isBaseOrb)
        {
            for (int i = 0; i < amount; i++)
            {
                Orb randomOrb = FindRandomOrbInList(orbs);
                Vector3 randomOrbPosition = randomOrb.transform.position;
                TransientOrb tempOrb = Instantiate(orb, randomOrbPosition, Quaternion.identity);
                orbs.Remove(randomOrb);

                //only needs an anchor if the replaced Orb is a basemanaorb
                //every other orb should be anchored to a basemanaorb themselves
                if (isBaseOrb)
                {
                    tempOrb.SetAnchorOrb(randomOrb);
                    randomOrb.SetInactive();
                }
                else
                {
                    //ToDo: get AnchorOrb of TransientOrb and set that orb as anchor orb to the new transient orb
                    //Problem: randomOrb is of type Orb, which doesn't have an AnchorOrb, only TransientOrbs do
                    Destroy(randomOrb.gameObject);
                }                            
            }
        }
        #endregion

        private enum SearchTag
        {
            BaseOrbs,
            IsUnoccupied,
            IsActive,
        }
    }
}
