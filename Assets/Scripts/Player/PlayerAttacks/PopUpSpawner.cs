using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleAttacks.AttackVisuals.PopUps
{
    public class PopUpSpawner : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private DamagePopUp _popUpPrefab;

        #endregion

        #region Public Functions

        public void SpawnPopUp(int damageAmount)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            float xOffset = transform.position.x + spriteRenderer.size.x / 2f;
            float yOffset = transform.position.y + spriteRenderer.size.y / 2f;

            Vector3 spawnPosition = new(xOffset, yOffset, transform.position.z);
            DamagePopUp tempPopUp = Instantiate(_popUpPrefab, spawnPosition, Quaternion.identity);
            tempPopUp.Setup(damageAmount);
        }

        #endregion
    }
}
