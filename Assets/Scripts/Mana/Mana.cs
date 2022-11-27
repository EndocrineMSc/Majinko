using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PeggleMana
{
    public class Mana : MonoBehaviour
    {
        #region Fields

        [SerializeField] private ManaType _manaType;

        public ManaType Type { get; private set; }

        #endregion
    }
}