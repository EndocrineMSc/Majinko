using Cards;
using DG.Tweening;
using Relics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    internal class RelicShop : MonoBehaviour
    {
        #region Fields and Properties

        private Canvas _shopCanvas;
        private RelicManager _relicManager;
        [SerializeField] private Button _shopRelicButton;
        [SerializeField] private TextMeshProUGUI _relicDescription;
        [SerializeField] private RelicRarityList _commonRelics;
        [SerializeField] private RelicRarityList _rareRelics;
        [SerializeField] private RelicRarityList _epicRelics;
        [SerializeField] private RelicRarityList _legendaryRelics;

        internal float RareThreshold { get; private set; } = 102;   //ToDo: change when relics exist
        internal float EpicThreshold { get; private set; } = 102;   //ToDo: change when relics exist
        internal float LegendaryThreshold { get; private set; } = 102;  //ToDo: change when relics exist

        #endregion

        #region Functions

        private void OnEnable()
        {
            UtilityEvents.OnLevelVictory += OnLevelVictory;
        }

        private void OnDisable()
        {
            UtilityEvents.OnLevelVictory -= OnLevelVictory;    
        }

        private void Start()
        {
            SetReferences();
        }

        private void SetReferences()
        {
            _shopCanvas = GetComponent<Canvas>();
            _shopCanvas.enabled = false;
            _relicManager = RelicManager.Instance;
        }

        private void OnLevelVictory()
        {
            _shopCanvas.enabled = true;
            Relic chosenRelic = SetRandomShopRelic();
            SetupShopRelic(chosenRelic);
        }

        private Relic SetRandomShopRelic()
        {
            var retries = 0;
            List<Relic> rarityList = new();

            while (rarityList.Count <= 0)
            {
                if (retries > 50)
                {
                    Debug.Log("Shop Building Failed");
                    return Relic.None;
                }
                rarityList = DetermineRelicRarityList();
                retries++;
            }

            return GetRandomRelicFromList(rarityList);
        }

        private List<Relic> DetermineRelicRarityList()
        {
            int randomRarityThreshold = UnityEngine.Random.Range(0, 101);

            if (randomRarityThreshold > LegendaryThreshold && _legendaryRelics.RelicList.Count > 0)
            {
                return _legendaryRelics.RelicList;
            }
            else if (randomRarityThreshold > EpicThreshold && _epicRelics.RelicList.Count > 0)
            {
                return _epicRelics.RelicList;
            }
            else if (randomRarityThreshold > RareThreshold && _rareRelics.RelicList.Count > 0)
            {
                return _rareRelics.RelicList;
            }
            else
            {
                return _commonRelics.RelicList;
            }
        }

        private Relic GetRandomRelicFromList(List<Relic> relicList)
        {
            int whileBreaker = 0;

            while (whileBreaker < 50)
            {
                int randomRelicIndex = UnityEngine.Random.Range(0, relicList.Count);
                Relic randomRelic = relicList[randomRelicIndex];
                
                if (!_relicManager.ActiveRelics.Contains(randomRelic))
                    return randomRelic;
                else 
                    whileBreaker++;
            }

            return Relic.None;
        }

        private void SetupShopRelic(Relic chosenRelic)
        {
            if (chosenRelic != Relic.None)
            {
                var relicSource = _relicManager.AllRelics[chosenRelic];
                var relicImage = relicSource.GetComponent<Image>();

                _relicDescription.text = relicSource.GetComponent<IRelic>().Description;
                _shopRelicButton.GetComponent<Image>().sprite = relicImage.sprite;
                _shopRelicButton.onClick.AddListener(delegate { ChooseRelic(chosenRelic); });
                _shopRelicButton.onClick.AddListener(delegate { StartCoroutine(DisableShopWithDelay()); });
                _shopRelicButton.onClick.AddListener(delegate { DisableAllButtons(_shopRelicButton); });
                _shopRelicButton.onClick.AddListener(TweenButtonClick);
            }
        }

        private void ChooseRelic(Relic relic)
        {
            _relicManager.AddRelic(relic);
        }

        private IEnumerator DisableShopWithDelay()
        {
            yield return new WaitForSeconds(1);
            _shopCanvas.enabled = false;
        }

        private void DisableAllButtons(Button button)
        {
            button.interactable = false;
        }

        private void TweenButtonClick()
        {
            _shopRelicButton.transform.DOPunchScale(new(1.1f, 1.1f), 0.2f, 1, 1);
        }

        #endregion
    }
}
