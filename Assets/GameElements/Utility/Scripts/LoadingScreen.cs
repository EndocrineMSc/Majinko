using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Orbs;

namespace Utility
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private OrbData[] _allOrbData;
        [SerializeField] private GameObject _basicOrbPrefab;


        private void Start()
        {
            InstantiateRandomOrb();
            StartCoroutine(WaitForLoadingScreen());
        }

        private IEnumerator WaitForLoadingScreen()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(LoadHelper.SceneToBeLoaded.ToString());
        }

        private void InstantiateRandomOrb()
        {
            int randomIndex = UnityEngine.Random.Range(0, _allOrbData.Length);
            var orbData = _allOrbData[randomIndex];
            var orbObject = Instantiate(_basicOrbPrefab, Vector3.zero, Quaternion.identity);
            orbObject.transform.localScale *= 2;
            orbObject.GetComponent<Orb>().SetOrbData(orbData);
        }
    }
}
