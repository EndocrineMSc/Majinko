using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleAttacks.AttackVisuals.PopUps
{
    internal class PopUpSpawner : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private DamagePopUp _popUpPrefab;

        #endregion

        #region Functions

        internal void SpawnPopUp(int damageAmount)
        {
            Transform spawnTransform = transform.GetChild(1).gameObject.transform;
            float xOffset = spawnTransform.position.x + UnityEngine.Random.Range(-0.1f, 0.1f);
            float yOffset = spawnTransform.position.y + UnityEngine.Random.Range(-0.1f, 0.1f);

            Vector3 spawnPosition = new(xOffset, yOffset, spawnTransform.position.z);
            DamagePopUp tempPopUp = Instantiate(_popUpPrefab, spawnPosition, Quaternion.identity);
            tempPopUp.Setup(damageAmount);
        }

        internal void SpawnPopUp(string bark)
        {
            Transform spawnTransform = transform.GetChild(1).gameObject.transform;
            float xOffset = spawnTransform.position.x + UnityEngine.Random.Range(-0.1f, 0.1f);
            float yOffset = spawnTransform.position.y + UnityEngine.Random.Range(-0.1f, 0.1f);

            Vector3 spawnPosition = new(xOffset, yOffset, spawnTransform.position.z);
            DamagePopUp tempPopUp = Instantiate(_popUpPrefab, spawnPosition, Quaternion.identity);
            tempPopUp.Setup(bark);
        }

        #endregion
    }
}
