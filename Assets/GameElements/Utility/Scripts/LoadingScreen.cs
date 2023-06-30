using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    internal class LoadingScreen : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(WaitForLoadingScreen());
        }

        private IEnumerator WaitForLoadingScreen()
        {
            Debug.Log("Loading scene: " + LoadHelper.SceneToBeLoaded.ToString());
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(LoadHelper.SceneToBeLoaded.ToString());
        }
    }
}
