using UnityEngine;
using Characters.Enemies;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/PinConeOrbData")]
    public class PineConeOrbData : OrbData
    {
        [SerializeField] private GameObject _pineMousePrefab;
        public override void OrbEffect()
        {
            var enemyManager = EnemyManager.Instance;
            
            if (enemyManager != null)
            {
                var enemySpawnManager = enemyManager.GetComponent<EnemySpawnManager>();
                var enemyPositions = enemyManager.EnemyPositions;
                bool spawnSuccessfull = false;
                var whileCounter = 0;

                while (!spawnSuccessfull)
                {
                    int randomIndex = UnityEngine.Random.Range(0, enemyPositions.GetLength(1) - 1);
                    Vector2 testPosition = enemyPositions[0, randomIndex];

                    spawnSuccessfull = enemySpawnManager.SpawnEnemy(_pineMousePrefab, testPosition);

                    whileCounter++;

                    if (whileCounter > 50)
                    {
                        Debug.Log("OrbData: PineMouse Spawn Failed, while Counter exceeded 50! Are all positions full?");
                        break;
                    }
                }

            }
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Pine Cone Orb</b><size=20%>\n\n<size=100%>When hit, " +
                "will spawn a <b>Pine Mouse</b> enemy onto the battlefield immediately." +
                "Seems to have been shaken loose from a tree somehow...";
        }
    }
}
