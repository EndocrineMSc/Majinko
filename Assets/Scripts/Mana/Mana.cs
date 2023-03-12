using UnityEngine;
using EnumCollection;

namespace PeggleWars.ManaManagement
{
    internal class Mana : MonoBehaviour
    {
        #region Fields

        [SerializeField] private ManaType _manaType;

        internal ManaType Type { get; private set; }

        #endregion
    }
}