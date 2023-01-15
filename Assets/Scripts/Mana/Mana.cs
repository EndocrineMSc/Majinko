using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PeggleWars.ManaManagement
{
    public class Mana : MonoBehaviour
    {
        #region Fields

        [SerializeField] private ManaType _manaType;

        public ManaType Type { get; private set; }

        #endregion
    }
}