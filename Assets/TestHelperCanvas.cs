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



        private void Start()
        {
            _resetSavesButton.onClick.AddListener(ResetSaves);
            _leyLinesButton.onClick.AddListener(LoadLeyLines);
        }




        internal void ResetSaves()
        {
            UtilityEvents.RaiseGameReset();
        }

        internal void LoadLeyLines()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.Event_LeyLines);
        }
    }
}
