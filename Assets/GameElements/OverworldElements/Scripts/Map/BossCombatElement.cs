using Utility;

namespace Overworld
{
    internal class BossCombatElement : OverworldElement
    {
        internal override void TriggerSceneTransition()
        {
            switch (GlobalWorldManager.Instance.WorldIndex)
            {
                case 1:
                    LoadHelper.LoadSceneWithLoadingScreen(SceneName.LevelOne);
                    break;
            }
        }
    }
}
