using UnityEngine;
using UnityEngine.UI;
using Utility;

internal class TestButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    { 
        button = GetComponent<Button>(); 
    }

    private void Start()
    {
        button.onClick.AddListener(TestFunction);
    }

    private void TestFunction()
    {
        LoadHelper.LoadSceneWithLoadingScreen(SceneName.NormalCombat);
    }
}
