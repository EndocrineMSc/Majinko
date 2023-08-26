using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    internal class PlayerEvents : MonoBehaviour
    {
        internal static event Action OnGainedShield;


        internal static void RaiseGainedShield()
        {
            OnGainedShield?.Invoke();
        }
    }
}
