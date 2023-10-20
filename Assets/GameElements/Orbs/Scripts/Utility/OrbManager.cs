using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using DG.Tweening;
using System.Collections;
using Utility.TurnManagement;
using ManaManagement;
using Utility;

namespace Orbs
{
    public class OrbManager : MonoBehaviour
    {
        #region Fields and Properties

        public static OrbManager Instance { get; private set; }

        //Orb Lists
        public List<Orb> ActiveBasicOrbPool = new();
        public List<Orb> InactiveBasicOrbPool = new();
        public List<Orb> ActiveNonBasicOrbPool = new();
        public List<Orb> InactiveNonBasicOrbs = new();

        //Standard References
        [SerializeField] private Orb _basicOrbPrefab;
        [SerializeField] private OrbData _basicOrbData; //Will be used to reset orbs after being hit and for Initialization
        [SerializeField] private OrbData _refreshOrbData; //Will be used to re-employ refresh orbs when none are present
        [SerializeField] private OrbLayoutSet _worldOneLayouts;
       
        //Tweening
        private Vector2 _levelOrbSpawnPosition; //level orbs will start tween from this position
        private readonly string LEVEL_ORB_TAG = "LevelOrbSpawn";
        private readonly float _tweenDuration = 0.75f;
        private readonly float _tweenScaleZoom = 12f;

        //Gathered Mana Visualization
        public float GatheredBasicManaAmountTurn { get; private set; } = 0;
        public float GatheredFireManaAmountTurn { get; private set; } = 0;
        public float GatheredIceManaAmountTurn { get; private set; } = 0;

        //other
        private bool _isCheckingForRefreshOrbs;

        #endregion

        #region Functions

        private void OnEnable()
        {
            OrbEvents.SpawnMana += OnManaSpawn;
            OrbEvents.OnSetOrbsActive += SetAllOrbsActive;
            LevelPhaseEvents.OnStartShootingPhase += OnStartShooting;
        }

        private void OnDisable()
        {
            OrbEvents.SpawnMana -= OnManaSpawn;
            OrbEvents.OnSetOrbsActive -= SetAllOrbsActive;
            LevelPhaseEvents.OnStartShootingPhase -= OnStartShooting;

            ExodiaWinCondtionTracker.ResetExodia();
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            _levelOrbSpawnPosition = GameObject.FindGameObjectWithTag(LEVEL_ORB_TAG).transform.position;
            SetUpOrbArena();          
            StartCoroutine(SetUpLevelLoadOrbs()); //also starts card phase on resolving
        }

        private void SetUpOrbArena()
        {
            int randomLayoutIndex = UnityEngine.Random.Range(0, _worldOneLayouts.OrbLayouts.Length);
            var orbLayout = _worldOneLayouts.OrbLayouts[randomLayoutIndex];
            var orbPositions = orbLayout.OrbPositions;

            foreach (var orbPosition in orbPositions)
            {
                var newOrb = Instantiate(_basicOrbPrefab, orbPosition, Quaternion.identity);
                newOrb.SetOrbData(_basicOrbData);
                ActiveBasicOrbPool.Add(newOrb);
            }
        }

        private IEnumerator SetUpLevelLoadOrbs()
        {
            foreach (var orbData in GlobalOrbManager.Instance.LevelLoadOrbs)
            {
                ReplaceActiveBasicOrb(orbData, _levelOrbSpawnPosition);
                yield return new WaitForSeconds(_tweenDuration);
            }
            PhaseManager.Instance.StartCardPhase();
        }


        public void SwitchOrbs(OrbData orbData, Vector3 instantiatePosition, int switchAmount = 1)
        {
            for (int i = 0; i < switchAmount; i++)
            {
                bool successfullReplacement = ReplaceActiveBasicOrb(orbData, instantiatePosition);

                if (!successfullReplacement)
                    successfullReplacement = ReplaceActiveNonBasicOrb(orbData, instantiatePosition);

                if (!successfullReplacement)
                    successfullReplacement = ReplaceInactiveBasicOrb(orbData, instantiatePosition);

                if (!successfullReplacement)
                {
                    Debug.Log("Orb replacement went wrong. Debug! This isn't likely to happen!");
                    break;
                }
            }
        }

        private bool ReplaceActiveBasicOrb(OrbData orbData, Vector2 tweenStartPosition)
        {
            bool replacementSuccessfull = true;
            
            if (ActiveBasicOrbPool.Count > 0)
            {
                var tweenOrb = InstantiateTweenDouble(orbData, tweenStartPosition);
                int randomIndex = UnityEngine.Random.Range(0, ActiveBasicOrbPool.Count);
                var randomOrb = ActiveBasicOrbPool[randomIndex];

                StartCoroutine(TweenOrb(tweenOrb, randomOrb.transform.position));

                ActiveBasicOrbPool.RemoveAt(randomIndex);
                ActiveNonBasicOrbPool.Add(randomOrb);

                randomOrb.SetOrbData(orbData);
                return replacementSuccessfull;
            }
            else
            {
                return !replacementSuccessfull;
            }
        }

        private bool ReplaceActiveNonBasicOrb(OrbData orbData, Vector2 tweenStartPosition)
        {
            bool replacementSuccessfull = true;

            if (ActiveNonBasicOrbPool.Count > 0)
            {
                var tweenOrb = InstantiateTweenDouble(orbData, tweenStartPosition);
                int randomIndex = UnityEngine.Random.Range(0, ActiveNonBasicOrbPool.Count);
                var randomOrb = ActiveNonBasicOrbPool[randomIndex];

                StartCoroutine(TweenOrb(tweenOrb, randomOrb.transform.position));

                ActiveNonBasicOrbPool.RemoveAt(randomIndex);
                ActiveNonBasicOrbPool.Add(randomOrb);

                randomOrb.SetOrbData(orbData);
                return replacementSuccessfull;
            }
            else
            {
                return !replacementSuccessfull;
            }
        }

        private bool ReplaceInactiveBasicOrb(OrbData orbData, Vector2 tweenStartPosition)
        {
            bool replacementSuccessfull = true;

            if (InactiveBasicOrbPool.Count > 0)
            {
                var tweenOrb = InstantiateTweenDouble(orbData, tweenStartPosition);
                int randomIndex = UnityEngine.Random.Range(0, InactiveBasicOrbPool.Count);
                var randomOrb = InactiveBasicOrbPool[randomIndex];

                StartCoroutine(TweenOrb(tweenOrb, randomOrb.transform.position));

                InactiveBasicOrbPool.RemoveAt(randomIndex);
                ActiveNonBasicOrbPool.Add(randomOrb);

                randomOrb.SetOrbData(orbData);
                randomOrb.SetOrbActive();
                return replacementSuccessfull;
            }
            else
            {
                return !replacementSuccessfull;
            }
        }

        private Orb InstantiateTweenDouble(OrbData orbData, Vector2 tweenStartPosition)
        {
            var tempOrb = Instantiate(_basicOrbPrefab, tweenStartPosition, Quaternion.identity);
            tempOrb.SetOrbData(orbData);
            tempOrb.GetComponent<Collider2D>().enabled = false;
            return tempOrb;
        }

        private IEnumerator TweenOrb(Orb orb, Vector3 targetPosition)
        {
            Vector3 endScale = orb.transform.localScale;
            orb.transform.localScale = new Vector3((endScale.x + _tweenScaleZoom), (endScale.y + _tweenScaleZoom), (endScale.z + _tweenScaleZoom));
            orb.transform.DOLocalMove(targetPosition, _tweenDuration).SetEase(Ease.InExpo);
            orb.transform.DOScale(endScale, _tweenDuration).SetEase(Ease.InExpo);
            yield return new WaitForSeconds(_tweenDuration);

            if (orb.transform.childCount > 0)
                orb.transform.GetComponentInChildren<ParticleSystem>().Play();

            orb.transform.DOPunchScale(endScale * 1.05f, 0.2f, 1, 1);
            yield return new WaitForSeconds(0.201f);
            Destroy(orb.gameObject);

            if (ScreenShaker.Instance != null)
                ScreenShaker.Instance.ShakeCamera(0.5f, 0.05f);
        }

        public void CheckForRefreshOrbs()
        {
            //bool wrapper to prevent double instantiation of orb due to multiple triggering of function
            //not sure why this is happening, but this is a quick fix
            if (!_isCheckingForRefreshOrbs)
            {
                _isCheckingForRefreshOrbs = true;
                int refreshOrbsInScene = 0;

                foreach (Orb orb in ActiveNonBasicOrbPool)
                    if (orb.Data.name.Contains("Refresh"))
                        refreshOrbsInScene++;

                if (refreshOrbsInScene < GlobalOrbManager.Instance.AmountOfRefreshOrbs)
                {
                    int refreshOrbDelta = GlobalOrbManager.Instance.AmountOfRefreshOrbs - refreshOrbsInScene;
                    SwitchOrbs(_refreshOrbData, transform.position, refreshOrbDelta);
                }
                _isCheckingForRefreshOrbs = false;
            }           
        }

        private void OnManaSpawn(ManaType manaType, int amount)
        {
            int modifier = ManaPool.Instance.ManaCostMultiplier;

            if (amount != 0)
            {
                switch (manaType)
                {
                    case ManaType.BasicMana:
                        GatheredBasicManaAmountTurn += (float)amount / modifier;
                        break;
                    case ManaType.FireMana:
                        GatheredFireManaAmountTurn += (float)amount / modifier;
                        break;
                    case ManaType.IceMana:
                        GatheredIceManaAmountTurn += (float)amount / modifier;
                        break;
                    case ManaType.RottedMana:
                        GatheredBasicManaAmountTurn += (float)amount / modifier;  
                        break;
                }
            }
        }

        private void OnStartShooting()
        {
            GatheredBasicManaAmountTurn = 0;
            GatheredFireManaAmountTurn = 0;
            GatheredIceManaAmountTurn = 0;
            ArenaConditionTracker.ResetHitOrbsInTurn();
        }      

        public void ReturnOrbToDisabledPool(Orb orb)
        {
            orb.SetOrbInactive();

            if (orb.Data.name.Contains("Base"))
            {
                ActiveBasicOrbPool.Remove(orb);
                InactiveBasicOrbPool.Add(orb);
            }
            else if (orb.Data.RevertsToBasicOrb)
            { 
                orb.SetOrbData(_basicOrbData);
                ActiveNonBasicOrbPool.Remove(orb);
                InactiveBasicOrbPool.Add(orb);
            }
            else
            {
                ActiveNonBasicOrbPool.Remove(orb);
                InactiveNonBasicOrbs.Add(orb);
            }            
        }

        public void SetAllOrbsActive()
        {
            foreach (var orb in InactiveBasicOrbPool)
            {
                ActiveBasicOrbPool.Add(orb);
                orb.SetOrbActive();
            }
            InactiveBasicOrbPool.Clear();

            foreach (var orb in InactiveNonBasicOrbs)
            {
                ActiveNonBasicOrbPool.Add(orb);
                orb.SetOrbActive();
            }
            InactiveNonBasicOrbs.Clear();
        }

        
        public void SetRandomOrbInactive()
        {
            if (ActiveBasicOrbPool.Count > 0) 
            {
                Orb randomOrb;
                int randomIndex;
                if (ActiveNonBasicOrbPool.Count > 0)
                {
                    int randomList = UnityEngine.Random.Range(0, 2);

                    if (randomList == 0)
                    {
                        randomIndex = UnityEngine.Random.Range(0, ActiveBasicOrbPool.Count);
                        randomOrb = ActiveBasicOrbPool[randomIndex];
                        ActiveBasicOrbPool.RemoveAt(randomIndex);
                        InactiveBasicOrbPool.Add(randomOrb);
                    }
                    else
                    {
                        randomIndex = UnityEngine.Random.Range(0, ActiveNonBasicOrbPool.Count);
                        randomOrb = ActiveNonBasicOrbPool[randomIndex];
                        randomOrb.SetOrbInactive();
                        ActiveNonBasicOrbPool.RemoveAt(randomIndex);
                        InactiveNonBasicOrbs.Add(randomOrb);
                    }                 
                }
                else
                {
                    randomIndex = UnityEngine.Random.Range(0, ActiveBasicOrbPool.Count);
                    randomOrb = ActiveBasicOrbPool[randomIndex];
                    ActiveBasicOrbPool.RemoveAt(randomIndex);
                    InactiveBasicOrbPool.Add(randomOrb);
                }
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
