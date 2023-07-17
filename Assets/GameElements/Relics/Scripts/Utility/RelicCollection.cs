using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Relics
{
    [CreateAssetMenu(menuName = "Relic Colletion")]
    internal class RelicCollection : ScriptableObject
    {
        #region Fields and Properties

        internal Dictionary<Relic, GameObject> AllRelics { get; private set; }

        [SerializeField] private List<Relic> _sortedEnum;
        [SerializeField] private List<GameObject> _sortedObjects;

        #endregion

        #region Functions

        private void OnValidate()
        {
            _sortedEnum ??= new();
            _sortedObjects ??= new();
            AllRelics ??= new();

            _sortedEnum = Enum.GetValues(typeof(Relic)).Cast<Relic>().ToList();
            _sortedEnum.Sort();
            _sortedObjects.Sort();

            for (int i = 0; i < _sortedEnum.Count; i++)
                if (_sortedObjects.Count >= _sortedEnum.Count && _sortedObjects[i] != null)
                    AllRelics.Add(_sortedEnum[i], _sortedObjects[i]);
        }

        #endregion
    }
}
