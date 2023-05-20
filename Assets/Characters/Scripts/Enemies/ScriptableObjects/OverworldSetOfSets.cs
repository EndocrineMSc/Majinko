using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/OverworldSetOfSets")]
    internal class OverworldSetOfSets : ScriptableObject
    {
        [SerializeField] internal ScriptableEnemySet[] ViableEnemySets;
    }
}
