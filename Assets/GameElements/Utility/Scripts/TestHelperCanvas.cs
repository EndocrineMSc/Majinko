using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class TestHelperCanvas : MonoBehaviour
    {
        [SerializeField] private Button _resetSavesButton;
        [SerializeField] private Button _leyLinesButton;
        [SerializeField] private Button _forestFireButton;
        [SerializeField] private Button _mirrorTreeButton;
        [SerializeField] private Button _mysteriousStrangerButton;
        [SerializeField] private Button _eliteCombatButton;
        [SerializeField] private Button _bossCombatButton;

        private void Start()
        {
            _resetSavesButton.onClick.AddListener(ResetSaves);
            _leyLinesButton.onClick.AddListener(LoadLeyLines);
            _forestFireButton.onClick.AddListener(LoadForestFire);
            _mirrorTreeButton.onClick.AddListener(LoadMirrorTree);
            _mysteriousStrangerButton.onClick.AddListener(LoadMysteriousStranger);
            _eliteCombatButton.onClick.AddListener(LoadEliteCombat);
            _bossCombatButton.onClick.AddListener(LoadBossCombat);
        }

        public void ResetSaves()
        {
            UtilityEvents.RaiseGameReset();
        }

        public void LoadLeyLines()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.Event_LeyLines);
        }

        public void LoadForestFire()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.Event_ForestFire);
        }

        public void LoadMirrorTree()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.Event_MirrorTree);
        }

        public void LoadMysteriousStranger()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.Event_MysteriousStranger);
        }

        public void LoadEliteCombat()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.EliteCombat);
        }

        public void LoadBossCombat()
        {
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.BossCombat);
        }
    }
}
