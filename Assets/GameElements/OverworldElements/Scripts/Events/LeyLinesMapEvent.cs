using UnityEngine;
using Orbs;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using EnumCollection;
using Utility;

namespace Overworld
{
    internal class LeyLinesMapEvent : MonoBehaviour
    {
        private OrbType _orbType;
        [SerializeField] private Button _addOrbButton;
        [SerializeField] private Button _leaveButton;

        private void Awake()
        {
            UtilityEvents.RaiseGameReset();          
        }

        private void Start()
        {
            List<Orb> persistentOrbs = new();
            foreach (Orb orb in GlobalOrbManager.Instance.AllOrbsList)
            {
                if (orb.TryGetComponent<IAmPersistent>(out _) && !orb.OrbType.ToString().Contains("Forbidden"))
                    persistentOrbs.Add(orb);
            }

            int randomIndex = UnityEngine.Random.Range(0, persistentOrbs.Count);
            _orbType = persistentOrbs[randomIndex].OrbType;

            _addOrbButton.onClick.AddListener(AddOrb);
            _leaveButton.onClick.AddListener(LeaveEvent);
        }

        internal void AddOrb()
        {
            _addOrbButton.interactable = false;
            GlobalOrbManager.Instance.AddLevelLoadOrb(_orbType);
            var orbObject = Instantiate(GlobalOrbManager.Instance.AllOrbsList[(int)_orbType], Camera.main.ScreenToWorldPoint(_addOrbButton.transform.position), Quaternion.identity);
            orbObject.GetComponent<Collider2D>().enabled = false;
            orbObject.transform.position = new(orbObject.transform.position.x, orbObject.transform.position.y, 0);
            orbObject.transform.localScale *= 3;
            orbObject.transform.DOJump(new Vector3(orbObject.transform.localPosition.x * 1.2f, orbObject.transform.localPosition.y * 5, 0), 1, 1, 2);
            StartCoroutine(LoadNextScene());           
        }

        internal void LeaveEvent()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.WorldOne);
        }
        
        private IEnumerator LoadNextScene()
        {
            if (FadeCanvas.Instance != null)
                FadeCanvas.Instance.FadeImage.DOFade(1, LoadHelper.LoadDuration);

            yield return new WaitForSeconds(LoadHelper.LoadDuration);
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.WorldOne);
        }
    }
}
