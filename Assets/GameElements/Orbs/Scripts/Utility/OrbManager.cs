using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnumCollection;
using DG.Tweening;
using System.Collections;
using Audio;
using Utility.TurnManagement;

namespace Orbs
{
    internal class OrbManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static OrbManager Instance { get; private set; }

        //Lists
        internal List<Orb> SceneOrbList { get; set; }
        private List<Orb> _allOrbsList;
        private List<ScriptableOrbLayout> _orbLayoutList;
       
        //Tweening
        private readonly float _tweenDuration = 0.5f;
        private readonly float _tweenScaleZoom = 8f;
        private GameObject _levelOrbSpawn;
        private readonly string LEVEL_ORB_TAG = "LevelOrbSpawn";

        //other
        private bool _isCheckingForRefreshOrbs;
        [SerializeField] private OrbLayoutSet _wordOneLayouts;

        #endregion

        #region Functions
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            InitializeLists();
            _orbLayoutList = _wordOneLayouts.OrbLayouts.ToList();
        }

        private void Start()
        {
            _levelOrbSpawn = GameObject.FindGameObjectWithTag(LEVEL_ORB_TAG);
            SetUpOrbArena();          
            StartCoroutine(InsertLevelLoadOrbs()); //also starts card phase on resolving
        }

        private void InitializeLists()
        {
            SceneOrbList ??= new();
            _allOrbsList ??= new();
            _orbLayoutList ??= new();
        }

        private void SetUpOrbArena()
        {
            OrbGridPositions orbGrid = new();
            _allOrbsList = GlobalOrbManager.Instance.AllOrbsList;
            ScriptableOrbLayout scriptableOrbLayout = GetRandomLevelLayout();
            bool[,] orbLayout = scriptableOrbLayout.InitializeOrbLayout();
            for (int x = 0; x < orbLayout.GetLength(0); x++)
            {
                for (int y = 0; y < orbLayout.GetLength(1); y++)
                {
                    if (orbLayout[x, y])
                    {
                        Vector2 instantiatePosition = orbGrid.GridArray[x, y];
                        Orb orb = Instantiate(_allOrbsList[0], instantiatePosition, Quaternion.identity);
                        SceneOrbList.Add(orb);
                    }
                }
            }
        }

        private ScriptableOrbLayout GetRandomLevelLayout()
        {
            int randomLayoutIndex = UnityEngine.Random.Range(0, _orbLayoutList.Count);
            ScriptableOrbLayout orbLayout = _orbLayoutList[randomLayoutIndex];
            return orbLayout;
        }

        private IEnumerator InsertLevelLoadOrbs()
        {
            yield return new WaitForSeconds(1); //so that player can see first animations, too
            foreach (Orb orb in GlobalOrbManager.Instance.LevelLoadOrbs)
            {
                SwitchOrbs(orb.OrbType, _levelOrbSpawn.transform.position);
                yield return new WaitForSeconds(_tweenDuration);
            }
            PhaseManager.Instance.StartCardPhase();
        }

        internal void SwitchOrbs(OrbType orbType, Vector3 instantiatePosition, int switchAmount = 1)
        {
            List<Orb> baseOrbs = FindOrbs(SceneOrbList, SearchTag.BaseOrbs);          
            List<Orb> activeBaseOrbs = FindOrbs(baseOrbs, SearchTag.IsActive);           
            Orb instantiateOrb = _allOrbsList[(int)orbType];

            //Case 1: enough active base orbs present, so switch some of those
            if (activeBaseOrbs.Count >= switchAmount)
            {
                StartCoroutine(ReplaceOrbsInList(activeBaseOrbs, switchAmount, instantiateOrb, instantiatePosition));
            }
            //Case 2: enough base orbs are present, but some of them are inactive, so switch the active ones first, then the inactive ones
            else if (baseOrbs.Count >= switchAmount)
            {
                int availableOrbs = activeBaseOrbs.Count;
                int missingOrbs = switchAmount - activeBaseOrbs.Count;

                if (availableOrbs != 0)
                {
                    StartCoroutine(ReplaceOrbsInList(activeBaseOrbs, availableOrbs, instantiateOrb, instantiatePosition)    );
                    baseOrbs = FindOrbs(SceneOrbList, SearchTag.BaseOrbs);
                }
                StartCoroutine(ReplaceOrbsInList(baseOrbs, missingOrbs, instantiateOrb, instantiatePosition));
            }
            //Case 3: there are only non-base orbs left (unlikely) so switch active ones first then inactive ones second
            else
            {
                List<Orb> activeOrbs = FindOrbs(SceneOrbList, SearchTag.IsActive);
                List<Orb> inactiveOrbs = FindOrbs(SceneOrbList, SearchTag.IsInactive);
                int availableOrbs = activeOrbs.Count;
                StartCoroutine(ReplaceOrbsInList(activeOrbs, availableOrbs, instantiateOrb, instantiatePosition));
                int missingOrbs = switchAmount - activeOrbs.Count;
                StartCoroutine(ReplaceOrbsInList(inactiveOrbs, missingOrbs, instantiateOrb, instantiatePosition));
            }
        }

        internal void CheckForRefreshOrbs()
        {
            //bool wrapper to prevent double instantiation of orb due to multiple triggering of function
            //not sure why this is happening, but this is a quick fix
            if (!_isCheckingForRefreshOrbs)
            {
                _isCheckingForRefreshOrbs = true;
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
                _isCheckingForRefreshOrbs = false;
            }           
        }

        private Orb FindRandomOrbInList(List<Orb> orbs)
        {
            if (orbs.Count == 0)
            {
                return null;
            }
            int randomOrbIndex = UnityEngine.Random.Range(0, orbs.Count);
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
                        if (tempOrb.IsOrbActive)
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
        
        private IEnumerator ReplaceOrbsInList(List<Orb> orbs, int amount, Orb orb, Vector3 startPosition)
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0770_Orb_Spawn_Whoosh);
            int safetyCounter = 0;
            for (int i = 0; i < amount; i++)
            {
                Orb randomOrb = FindRandomOrbInList(orbs);
                if (randomOrb != null)
                {
                    Vector3 randomOrbPosition = randomOrb.transform.position;
                    Orb tempOrb = Instantiate(orb, startPosition, Quaternion.identity);
                    tempOrb.gameObject.GetComponent<Collider2D>().enabled = false;

                    SceneOrbList.Remove(randomOrb);
                    Destroy(randomOrb.gameObject);

                    Vector3 endScale = tempOrb.transform.localScale;
                    tempOrb.transform.localScale = new Vector3((endScale.x + _tweenScaleZoom), (endScale.y + _tweenScaleZoom), (endScale.z + _tweenScaleZoom));
                    tempOrb.transform.DOLocalMove(randomOrbPosition, _tweenDuration).SetEase(Ease.InOutExpo);
                    tempOrb.transform.DOScale(endScale, _tweenDuration);
                    SceneOrbList.Add(tempOrb);
                    yield return new WaitForSeconds(_tweenDuration);
                    tempOrb.gameObject.GetComponent<Collider2D>().enabled = true;
                }
                else
                {
                    i--;
                    safetyCounter++;
                    if (safetyCounter >= 30)
                    {
                        break;
                    }
                }
            }
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
