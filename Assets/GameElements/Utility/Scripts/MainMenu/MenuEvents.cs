using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utility
{
    internal class MenuEvents : MonoBehaviour
    {
        internal static event Action OnMainMenuOpened;
        internal static event Action OnSettingsMenuOpened;
        internal static event Action OnCreditsMenuOpened;
        internal static event Action OnTutorialOpened;

        internal static void RaiseMainMenuOpened()
        {
            OnMainMenuOpened?.Invoke();
        }

        internal static void RaiseSettingsMenuOpened()
        {
            OnSettingsMenuOpened?.Invoke();
        }

        internal static void RaiseCreditsOpened()
        {
            OnCreditsMenuOpened?.Invoke();
        }

        internal static void RaiseTutorialOpened()
        {
            OnTutorialOpened?.Invoke();
        }
    }
}
