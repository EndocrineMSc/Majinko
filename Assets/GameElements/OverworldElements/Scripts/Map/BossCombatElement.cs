using Utility;

namespace Overworld
{
    internal class BossCombatElement : OverworldElement
    {
        protected override void LoadScene()
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
