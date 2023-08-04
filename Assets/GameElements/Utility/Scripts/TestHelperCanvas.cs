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
        [SerializeField] private Button _mirrorTreeButton;
        [SerializeField] private Button _mysteriousStrangerButton;
        [SerializeField] private Button _eliteCombatButton;

        private void Start()
        {
            _resetSavesButton.onClick.AddListener(ResetSaves);
            _leyLinesButton.onClick.AddListener(LoadLeyLines);
            _forestFireButton.onClick.AddListener(LoadForestFire);
            _mirrorTreeButton.onClick.AddListener(LoadMirrorTree);
            _mysteriousStrangerButton.onClick.AddListener(LoadMysteriousStranger);
            _eliteCombatButton.onClick.AddListener(LoadEliteCombat);
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

        internal void LoadMirrorTree()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.Event_MirrorTree);
        }

        internal void LoadMysteriousStranger()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.Event_MysteriousStranger);
        }

        internal void LoadEliteCombat()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.EliteCombat);
        }
    }
}
