using Utility;

namespace Overworld
{
    internal class CurrentPlayerWorldPosition
    {
        internal static int OverworldPlayerButtonIndex { get; private set; } = 0;

        internal static void SetPlayerButtonIndex(int index)
        {
            OverworldPlayerButtonIndex = index;
        }
    }
}
