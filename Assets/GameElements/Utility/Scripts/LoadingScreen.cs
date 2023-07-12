using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Orbs;

namespace Utility
{
    internal class LoadingScreen : MonoBehaviour
    {
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
            int randomIndex = UnityEngine.Random.Range(0, GlobalOrbManager.Instance.AllOrbsList.Count);
            Orb orb = GlobalOrbManager.Instance.AllOrbsList[randomIndex];
            var orbObject = Instantiate(orb, Vector3.zero, Quaternion.identity);
            orbObject.transform.localScale *= 2;
        }
    }
}
