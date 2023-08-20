using Attacks;
using Characters;

namespace ManaManagement
{
    internal class RottedMana : Mana
    {
        private void OnDestroy()
        {
            PlayerConditionTracker.AddSicknessStacks();
        }
    }
}
