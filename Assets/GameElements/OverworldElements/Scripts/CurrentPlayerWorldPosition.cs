using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class CurrentPlayerWorldPosition
{
    internal static int OverworldPlayerButtonIndex { get; private set; } = 0;

    internal static void SetPlayerButtonIndex(int index)
    {
        OverworldPlayerButtonIndex = index;
    }
}
