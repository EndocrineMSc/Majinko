using UnityEngine;

namespace Characters.UI
{
    internal class PopUpSpawner : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private DamagePopUp _popUpPrefab;
        private Color _defaultColor = Color.red;

        #endregion

        #region Functions

        internal void SpawnPopUp(int damageAmount, string hexColor = "#FF0A01")
        {
            Transform spawnTransform = transform.GetChild(0).gameObject.transform;
            float xOffset = spawnTransform.position.x + UnityEngine.Random.Range(-0.1f, 0.1f);
            float yOffset = spawnTransform.position.y + UnityEngine.Random.Range(-0.1f, 0.1f);

            Vector3 spawnPosition = new(xOffset, yOffset, spawnTransform.position.z);
            DamagePopUp tempPopUp = Instantiate(_popUpPrefab, spawnPosition, Quaternion.identity);
            tempPopUp.Setup(damageAmount, hexColor);
        }

        internal void SpawnPopUp(string bark)
        {
            Transform spawnTransform = transform.GetChild(0).gameObject.transform;
            float xOffset = spawnTransform.position.x + UnityEngine.Random.Range(-0.1f, 0.1f);
            float yOffset = spawnTransform.position.y + UnityEngine.Random.Range(-0.1f, 0.1f);

            Vector3 spawnPosition = new(xOffset, yOffset, spawnTransform.position.z);
            DamagePopUp tempPopUp = Instantiate(_popUpPrefab, spawnPosition, Quaternion.identity);
            tempPopUp.Setup(bark);
        }

        #endregion
    }
}
