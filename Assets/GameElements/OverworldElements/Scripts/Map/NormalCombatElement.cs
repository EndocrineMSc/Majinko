using Utility;

namespace Overworld
{
    internal class NormalCombatElement : OverworldElement
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
