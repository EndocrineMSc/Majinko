using PeggleWars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        SceneManager.LoadScene("LevelOne");
    }
}
