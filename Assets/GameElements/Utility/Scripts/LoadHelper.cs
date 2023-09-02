using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Utility
{
    internal class LoadHelper
    {
        internal static SceneName SceneToBeLoaded { get; private set; } = SceneName.WorldOne;
        internal static float LoadDuration { get; private set; } = 1f;
        internal static string CURRENTSCENE_SAVE_PATH { get; } = "CurrentScene";
        internal static bool CurrentlyBossCombat { get; private set; }

        internal static void LoadSceneWithLoadingScreen(SceneName sceneName)
        {
            if (sceneName != SceneName.MainMenu)
                ES3.Save<SceneName>(CURRENTSCENE_SAVE_PATH, sceneName);

            if (sceneName == SceneName.BossCombat)
            {
                CurrentlyBossCombat = true;
                sceneName = SceneName.EliteCombat;
            }
            else
            {
                CurrentlyBossCombat = false;
            }    

            SceneToBeLoaded = sceneName;
            SceneManager.LoadSceneAsync(SceneName.LoadingScreen.ToString());
        }

        internal static void DeleteSceneKey()
        {
            if (ES3.KeyExists(CURRENTSCENE_SAVE_PATH))
                ES3.DeleteKey(CURRENTSCENE_SAVE_PATH);
        }
    }

    internal enum SceneName
    {
        MainMenu,
        Tutorial,
        WorldOne,
        LoadingScreen,
        GameOver,
        NormalCombat,
        EliteCombat,
        BossCombat,
        Event_LeyLines,
        Event_ForestFire,
        Event_MagicalSpring,
        Event_MirrorTree,
        Event_MysteriousStranger,
    }
}
