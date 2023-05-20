using System;
using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbLayout"), Serializable]
    internal class ScriptableOrbLayout : ScriptableObject
    {
        [SerializeField] internal bool[] Row1 = new bool[21];
        [SerializeField] internal bool[] Row2 = new bool[21];
        [SerializeField] internal bool[] Row3 = new bool[21];
        [SerializeField] internal bool[] Row4 = new bool[21];
        [SerializeField] internal bool[] Row5 = new bool[21];
        [SerializeField] internal bool[] Row6 = new bool[21];
        [SerializeField] internal bool[] Row7 = new bool[21];
        [SerializeField] internal bool[] Row8 = new bool[21];
        [SerializeField] internal bool[] Row9 = new bool[21];
        [SerializeField] internal bool[] Row10 = new bool[21];
        [SerializeField] internal bool[] Row11 = new bool[21];
        [SerializeField] internal bool[] Row12 = new bool[21];
        [SerializeField] internal bool[] Row13 = new bool[21];

        private bool[,] _orbLayout = new bool[21, 13];

        internal bool[,] InitializeOrbLayout()
        {
            SetRow(1, Row1);
            SetRow(2, Row2);
            SetRow(3, Row3);
            SetRow(4, Row4);
            SetRow(5, Row5);
            SetRow(6, Row6);
            SetRow(7, Row7);
            SetRow(8, Row8);
            SetRow(9, Row9);
            SetRow(10, Row10);
            SetRow(11, Row11);
            SetRow(12, Row12);
            SetRow(13, Row13);

            return _orbLayout;
        }

        private void SetRow(int layoutRow, bool[] row)
        {
            for (int i = 0; i < row.Length; i++)
            {
                _orbLayout[i, layoutRow - 1] = row[i];
            }
        }
    }
}

