using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


namespace BloodBehaviuor
{
    public class BloodCells : MonoBehaviour
    {
        #region Fields

        [SerializeField] private LiquidBlood _liquidBlood;
        private float _spawnOffset = 0.02f;

        #endregion

        #region Properties




        #endregion

        #region Public Functions





        #endregion


        #region Private Functions

        private void Start()
        {
            //Ignore the collisions between layer 0 (default) and layer 8 (custom layer you set in Inspector window)
            Physics.IgnoreLayerCollision(6, 7);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains("Shot"))
            {
                gameObject.GetComponent<SpriteRenderer>().size += new Vector2(0.03f, 0.03f);
                StartCoroutine(nameof(SetInactive));
                

                //spawns i blood particles in a roughly ball shaped form -> to enable shader blur, visualizing the blood properly
                for (int i = 0; i < 10; i++)
                {
                    if ((i % 2) == 0 && i < 5)
                    {
                        Instantiate(_liquidBlood, new Vector3(transform.position.x + (i * _spawnOffset), transform.position.y - (i * _spawnOffset), transform.position.z), Quaternion.identity);
                    }
                    else if ((i% 2) == 1 && i < 5)
                    {
                        Instantiate(_liquidBlood, new Vector3(transform.position.x - (i * _spawnOffset), transform.position.y - (i * _spawnOffset), transform.position.z), Quaternion.identity);
                    }
                    else if ((i % 2) == 0 && i >= 5)
                    {
                        Instantiate(_liquidBlood, new Vector3(transform.position.x + ((9 - i) * _spawnOffset), transform.position.y - ((9-1) * _spawnOffset), transform.position.z), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(_liquidBlood, new Vector3(transform.position.x + ((9 - i) * _spawnOffset), transform.position.y - ((9 - 1) * _spawnOffset), transform.position.z), Quaternion.identity);
                    }
                
                }
             
            

            }
        }


        #endregion


        #region IEnumerators

        private IEnumerator SetInactive()
        {
            yield return new WaitForSeconds(0.1f);
            gameObject.GetComponent<SpriteRenderer>().size -= new Vector2(0.02f, 0.02f);
            gameObject.SetActive(false);
        }

        #endregion




    }
}

