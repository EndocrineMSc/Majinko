using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Utility.TurnManagement
{
    internal class LevelPhaseEvents
    {
        internal static event Action OnStartCardPhase;
        internal static event Action OnEndCardPhase;
        internal static event Action OnStartShootingPhase;
        internal static event Action OnStartEnemyPhase;
        internal static event Action OnEndEnemyPhase;
        internal static event Action OnStartPlayerAttackPhase;

        internal static void RaiseStartCardPhase()
        {
            OnStartCardPhase?.Invoke();
        }

        internal static void RaiseEndCardPhase()
        {
            OnEndCardPhase?.Invoke();
        }

        internal static void RaiseStartEnemyPhase() 
        { 
            OnStartEnemyPhase?.Invoke(); 
        }

        internal static void RaiseEndEnemyPhase() 
        { 
            OnEndEnemyPhase?.Invoke(); 
        }

        internal static void RaiseStartPlayerAttackPhase() 
        {
            OnStartPlayerAttackPhase?.Invoke(); 
        }

        internal static void RaiseStartShootingPhase()
        {
            OnStartShootingPhase?.Invoke();
        }
    }
}
