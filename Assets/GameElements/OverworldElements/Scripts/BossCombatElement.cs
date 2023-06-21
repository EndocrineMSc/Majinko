using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Overworld
{
    internal class BossCombatElement : OverworldElement
    {
        #region Fields and Propertieds

        private const string LEVELONE_PARAM = "LevelOne";

        #endregion

        #region Functions

        internal override void TriggerSceneTransition()
        {
            switch (GlobalWorldManager.Instance.WorldIndex)
            {
                case 1:
                    SceneManager.LoadSceneAsync(LEVELONE_PARAM);
                    break;
            }
        }

        #endregion
    }
}
