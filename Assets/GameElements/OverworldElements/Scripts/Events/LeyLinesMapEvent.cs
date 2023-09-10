using UnityEngine;
using Orbs;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using Utility;

namespace Overworld
{
    public class LeyLinesMapEvent : MonoBehaviour
    {
        [SerializeField] private OrbData[] _persistentOrbs;
        [SerializeField] private GameObject _basicOrbPrefab;
        [SerializeField] private Button _addOrbButton;
        [SerializeField] private Button _leaveButton;

        private OrbData _chosenOrbData;

        private void Start()
        {
            int randomIndex = UnityEngine.Random.Range(0, _persistentOrbs.Length);
            _chosenOrbData = _persistentOrbs[randomIndex];

            _addOrbButton.onClick.AddListener(AddOrb);
            _leaveButton.onClick.AddListener(LeaveEvent);
        }

        public void AddOrb()
        {
            _addOrbButton.interactable = false;
            GlobalOrbManager.Instance.AddLevelLoadOrb(_chosenOrbData);
            var orbObject = Instantiate(_basicOrbPrefab, Camera.main.ScreenToWorldPoint(_addOrbButton.transform.position), Quaternion.identity);
            orbObject.GetComponent<Orb>().SetOrbData(_chosenOrbData);
            orbObject.GetComponent<Collider2D>().enabled = false;
            orbObject.transform.position = new(orbObject.transform.position.x, orbObject.transform.position.y, 0);
            orbObject.transform.localScale *= 3;
            orbObject.transform.DOJump(new Vector3(orbObject.transform.localPosition.x * 1.2f, orbObject.transform.localPosition.y * 5, 0), 1, 1, 2);
            StartCoroutine(LoadNextScene());           
        }

        public void LeaveEvent()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.WorldOne);
        }
        
        private IEnumerator LoadNextScene()
        {
            if (FadeCanvas.Instance != null)
                FadeCanvas.Instance.FadeToBlack();

            yield return new WaitForSeconds(LoadHelper.LoadDuration);
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.WorldOne);
        }
    }
}
