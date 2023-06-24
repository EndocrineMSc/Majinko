using System;

namespace Utility
{
    internal class TutorialEvents
    {
        internal static event Action<int> OnCurrentActionCompleted;
        internal static event Action OnTutorialFinished;

        internal static void RaiseCurrentActionCompleted(int index)
        {
            OnCurrentActionCompleted?.Invoke(index);
        }

        internal static void RaiseTutorialFinished()
        {
            OnTutorialFinished?.Invoke();
        }
    }
}
