using Utility;
using System.Collections;
using Characters.Enemies;
using System;
using UnityEngine;
using EnumCollection;

namespace Orbs
{
    internal class PineConeOrb : Orb
    {
        private EnemyManager _enemyManager;
        private EnemySpawnManager _enemySpawnManager;

        protected override void SetReferences()
        {
            base.SetReferences();
            _enemyManager = EnemyManager.Instance;

            if ( _enemyManager != null )
                _enemySpawnManager = _enemyManager.GetComponent<EnemySpawnManager>();
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Pine Cone Orb</b><size=20%>\n\n<size=100%>Upon being hit, this orb spawns a <b>Pinconian</b> onto the battlefield.";
        }

        internal override IEnumerator OrbEffect()
        {
            if (_enemyManager != null)
            {
                var enemyPositions = _enemyManager.EnemyPositions;
                bool spawnSuccessfull = false;
                var whileCounter = 0;

                while (!spawnSuccessfull)
                {
                    int randomIndex = UnityEngine.Random.Range(0, enemyPositions.GetLength(1) - 1);
                    Vector2 testPosition = enemyPositions[0, randomIndex];

                    if (_enemySpawnManager != null)
                        spawnSuccessfull = _enemySpawnManager.SpawnEnemy(EnemyType.Shroombie, testPosition);

                    whileCounter++;

                    if (whileCounter > 50)
                    {
                        Debug.Log("Enemy Spawn Failed");
                        Debug.Log("Spawn Manager Reference == null: " + (_enemySpawnManager == null).ToString());
                        break;
                    }
                }

            }
            yield return null;
        }

        protected override void AdditionalEffectsOnCollision()
        {
            StartCoroutine(OrbEffect());
        }
    }
}
