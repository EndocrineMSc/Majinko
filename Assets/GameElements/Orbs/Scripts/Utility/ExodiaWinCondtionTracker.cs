namespace Utility
{
    public class ExodiaWinCondtionTracker
    {
        public static bool ForbiddenEActive { get; private set; }
        public static bool ForbiddenXActive { get; private set; }
        public static bool ForbiddenOActive { get; private set; }
        public static bool ForbiddenDActive { get; private set; }
        public static bool ForbiddenIActive { get; private set; }
        public static bool ForbiddenAActive { get; private set; }

        public static void SetForbiddenEActive() { ForbiddenEActive = true; }
        public static void SetForbiddenXActive() { ForbiddenXActive = true; }
        public static void SetForbiddenOActive() { ForbiddenOActive = true; }
        public static void SetForbiddenDActive() { ForbiddenDActive = true; }
        public static void SetForbiddenIActive() { ForbiddenIActive = true; }
        public static void SetForbiddenAActive() { ForbiddenAActive = true; }

        public static bool CheckForExodiaWin()
        {
            return (ForbiddenEActive && ForbiddenXActive
                && ForbiddenOActive && ForbiddenDActive
                && ForbiddenIActive && ForbiddenAActive);
        }

        public static void ResetExodia()
        {
            ForbiddenEActive = false;
            ForbiddenXActive = false;
            ForbiddenOActive = false;
            ForbiddenDActive = false;
            ForbiddenIActive = false;
            ForbiddenAActive = false;
        }
    }
}
