using UnityEngine;
using EnumCollection;

namespace ManaManagement
{
    public class Mana : MonoBehaviour
    {
        #region Fields

        [SerializeField] private ManaType _manaType;

        public ManaType Type { get; private set; }

        #endregion
    }
}