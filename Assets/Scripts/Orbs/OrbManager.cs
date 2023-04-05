using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnumCollection;
using System;
using PeggleWars.ManaManagement;
using DG.Tweening;
using System.Collections;

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

        private List<ScriptableOrbLayout> _orbLayoutList = new();
        private readonly string ORB_LAYOUT_PATH = "LevelOrbLayouts";
        
        private GlobalOrbManager _globalOrbManager;
        private readonly float _tweenDuration = 0.5f;
        private readonly float _tweenScaleZoom = 8f;

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
            _orbLayoutList = Resources.LoadAll<ScriptableOrbLayout>(ORB_LAYOUT_PATH).ToList();
        }

        private void Start()
        {
            _globalOrbManager = GlobalOrbManager.Instance;          

            OrbGridPositions orbGrid = new();
            _allOrbsList = _globalOrbManager.AllOrbsList;
            ScriptableOrbLayout scriptableOrbLayout = GetRandomLevelLayout();
            bool[,] orbLayout = scriptableOrbLayout.InitializeOrbLayout();
            for (int x = 0; x < orbLayout.GetLength(0); x++)
            {
                for (int y = 0; y < orbLayout.GetLength(1); y++)
                {
                    if (orbLayout[x, y])
                    {
                        Vector2 instantiatePosition = orbGrid.GridArray[x, y];
                        Instantiate(_allOrbsList[0], instantiatePosition, Quaternion.identity);
                    }
                    
                }
            }           
            SceneOrbList = GameObject.FindObjectsOfType<Orb>().ToList();
            StartCoroutine(InsertLevelLoadOrbs());
        }

        private ScriptableOrbLayout GetRandomLevelLayout()
        {
            int randomLayoutIndex = UnityEngine.Random.Range(0, _orbLayoutList.Count);
            ScriptableOrbLayout orbLayout = _orbLayoutList[randomLayoutIndex];
            return orbLayout;
        }

        private IEnumerator InsertLevelLoadOrbs()
        {
            foreach (Orb orb in _globalOrbManager.LevelLoadOrbs)
            {
                SwitchOrbs(orb.OrbType, transform.position);
                yield return new WaitForSeconds(_tweenDuration);
            }
            yield return null;
        }

        internal List<Orb> SwitchOrbs(OrbType orbType, Vector3 startPosition, int switchAmount = 1)
        {
            List<Orb> newOrbs = new();
            List<Orb> baseOrbs = FindOrbs(SceneOrbList, SearchTag.BaseOrbs);          
            List<Orb> activeBaseOrbs = FindOrbs(baseOrbs, SearchTag.IsActive);
           
            Orb orbToBeInserted = _allOrbsList[(int)orbType];

            //Case 1: enough active base orbs present, so switch some of those
            if (activeBaseOrbs.Count >= switchAmount)
            {
                newOrbs.AddRange(ReplaceOrbsInList(activeBaseOrbs, switchAmount, orbToBeInserted, startPosition));
            }
            //Case 2: enough base orbs are present, but some of them are inactive, so switch the active ones first, then the inactive ones
            else if (baseOrbs.Count >= switchAmount)
            {
                List<Orb> inactiveBaseOrbs = FindOrbs(baseOrbs, SearchTag.IsInactive);

                int availableOrbs = activeBaseOrbs.Count;
                newOrbs.AddRange(ReplaceOrbsInList(activeBaseOrbs, availableOrbs, orbToBeInserted, startPosition));

                int missingOrbs = switchAmount - activeBaseOrbs.Count;
                newOrbs.AddRange(ReplaceOrbsInList(inactiveBaseOrbs, missingOrbs, orbToBeInserted, startPosition));
            }
            //Case 3: there are only non-base orbs left (unlikely) so switch active ones first then inactive ones second
            else
            {
                List<Orb> activeOrbs = FindOrbs(SceneOrbList, SearchTag.IsActive);
                List<Orb> inactiveOrbs = FindOrbs(SceneOrbList, SearchTag.IsInactive);

                int availableOrbs = activeOrbs.Count;
                newOrbs.AddRange(ReplaceOrbsInList(activeOrbs, availableOrbs, orbToBeInserted, startPosition));

                int missingOrbs = switchAmount - activeOrbs.Count;
                newOrbs.AddRange(ReplaceOrbsInList(inactiveOrbs, missingOrbs, orbToBeInserted, startPosition));
            }
            return newOrbs;
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
            int refreshOrbsInScene = 0;

            foreach (Orb orb in SceneOrbList)
            {
                if (orb.OrbType == OrbType.RefreshOrb)
                {
                    refreshOrbsInScene++;
                }
            }

            if (refreshOrbsInScene < GlobalOrbManager.Instance.AmountOfRefreshOrbs)
            {
                int refreshOrbDelta = GlobalOrbManager.Instance.AmountOfRefreshOrbs - refreshOrbsInScene;
                SwitchOrbs(OrbType.RefreshOrb, transform.position, refreshOrbDelta);
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
        
        private List<Orb> ReplaceOrbsInList(List<Orb> orbs, int amount, Orb orb, Vector3 startPosition)
        {
            List<Orb> newOrbs = new();
            for (int i = 0; i < amount; i++)
            {
                Orb randomOrb = FindRandomOrbInList(orbs);
                Vector3 randomOrbPosition = randomOrb.transform.position;
                Orb tempOrb = Instantiate(orb, startPosition, Quaternion.identity);

                SceneOrbList.Remove(randomOrb);
                Destroy(randomOrb.gameObject);                            
                               
                Vector3 endScale = tempOrb.transform.localScale;
                tempOrb.transform.localScale = new Vector3((endScale.x + _tweenScaleZoom), (endScale.y + _tweenScaleZoom), (endScale.z + _tweenScaleZoom));
                tempOrb.transform.DOLocalMove(randomOrbPosition, _tweenDuration).SetEase(Ease.InOutExpo);
                tempOrb.transform.DOScale(endScale, _tweenDuration);

                SceneOrbList.Add(tempOrb);
                newOrbs.Add(tempOrb);             
            }
            return newOrbs;
        }

        internal void ReplaceOrbOfType(OrbType typeToBeReplaced, OrbType typeToReplaceItWith = OrbType.BaseManaOrb)
        {
            Orb orbToBeReplaced = null;            
            foreach (Orb orb in SceneOrbList)
            {
                if (orb.OrbType == typeToBeReplaced)
                {
                    orbToBeReplaced = orb;
                    break;
                }
            }
            if (orbToBeReplaced != null)
            {
                Vector3 orbPosition = orbToBeReplaced.transform.position;
                Orb replaceOrb = Instantiate(_allOrbsList[(int)typeToReplaceItWith], orbPosition, Quaternion.identity);
                SceneOrbList.Remove(orbToBeReplaced);
                SceneOrbList.Add(replaceOrb);
                Destroy(orbToBeReplaced.gameObject);
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
