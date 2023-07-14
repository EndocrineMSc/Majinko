using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    internal class LoadHelper
    {
        internal static SceneName SceneToBeLoaded { get; private set; } = SceneName.WorldOne;

        internal static void LoadSceneWithLoadingScreen(SceneName sceneName)
        {
            SceneToBeLoaded = sceneName;
            SceneManager.LoadSceneAsync(SceneName.LoadingScreen.ToString());
        }
    }

    internal enum SceneName
    {
        LevelOne,
        MainMenu,
        Tutorial,
        WorldOne,
        LoadingScreen,
        GameOver,
        Event_LeyLines,
        Event_ForestFire,
        Event_MagicalSpring,
    }
}
