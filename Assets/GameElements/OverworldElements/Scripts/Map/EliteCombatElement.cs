using Utility;

namespace Overworld
{
    internal class EliteCombatElement : OverworldElement
    {
        protected override void LoadScene()
        {
            switch (GlobalWorldManager.Instance.WorldIndex)
            {
                case 1:
                    LoadHelper.LoadSceneWithLoadingScreen(SceneName.EliteCombat);
                    break;
            }
        }
    }
}
