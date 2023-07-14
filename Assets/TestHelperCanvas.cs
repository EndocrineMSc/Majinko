using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    internal class TestHelperCanvas : MonoBehaviour
    {
        [SerializeField] private Button _resetSavesButton;
        [SerializeField] private Button _leyLinesButton;
        [SerializeField] private Button _forestFireButton;



        private void Start()
        {
            _resetSavesButton.onClick.AddListener(ResetSaves);
            _leyLinesButton.onClick.AddListener(LoadLeyLines);
            _forestFireButton.onClick.AddListener(LoadForestFire);
        }




        internal void ResetSaves()
        {
            UtilityEvents.RaiseGameReset();
        }

        internal void LoadLeyLines()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.Event_LeyLines);
        }

        internal void LoadForestFire()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.Event_ForestFire);
        }
    }
}
