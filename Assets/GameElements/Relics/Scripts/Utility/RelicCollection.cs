using System;
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
            BuildDictionary();
        }

        internal void BuildDictionary()
        {
            _sortedEnum ??= new();
            _sortedObjects ??= new();
            AllRelics ??= new();
            AllRelics.Clear();

            for (int i = 0; i < _sortedEnum.Count; i++)
                AllRelics.Add(_sortedEnum[i], _sortedObjects[i]);
        }

        private void DebugLogDictionary()
        {
            foreach (var item in AllRelics)
                Debug.Log("Key: " + item.Key.ToString() + "Value: " + item.Value.ToString());
        }


        #endregion
    }
}
