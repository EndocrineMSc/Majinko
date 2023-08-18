using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attacks
{
    internal class AttackEvents
    {
        internal static event Action OnAttackFinished;

        internal static void RaiseAttackFinished()
        {
            OnAttackFinished?.Invoke();
        }
    }
}
