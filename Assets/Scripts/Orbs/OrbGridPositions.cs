using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbGridPositions
{
    #region Fields

    private Vector2[,] _gridArray = new Vector2[21,13];

    public Vector2[,] GridArray { get { return _gridArray; } private set { _gridArray = value; } }

    #endregion

    #region Functions

    public OrbGridPositions()
    {
        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0;  y < _gridArray.GetLength(1); y++)
            {
                float xPosition = -7.5f + (x * 0.75f);
                float yPosition = -5.75f + (y * 0.75f);
                Vector2 orbPosition = new(xPosition, yPosition);

                _gridArray[x, y] = orbPosition;
            }
        }
    }

    #endregion
}
