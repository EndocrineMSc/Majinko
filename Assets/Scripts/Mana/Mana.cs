using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class Mana : MonoBehaviour
{
    #region Fields

    private GameObject[] SpawnArray;
    [SerializeField] private ManaType _manaType;

    #endregion

    void Start()
    {
        //Ignore the collisions between layer 0 (default) and layer 8 (custom layer you set in Inspector window)
        Physics.IgnoreLayerCollision(6, 7);

        SpawnArray = GameObject.FindGameObjectsWithTag("ManaSpawn");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Portal"))
        {
            Vector2 _spawnerPosition = SpawnArray[(int)_manaType].transform.position;
            float _randomSpawn = Random.Range(-0.7f, 0.7f);
            Vector2 _spawnPosition = new(_spawnerPosition.x +_randomSpawn, _spawnerPosition.y);

            Instantiate(this, _spawnPosition, Quaternion.identity);
        }
    }
}