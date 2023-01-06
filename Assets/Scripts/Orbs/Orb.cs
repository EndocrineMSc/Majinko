using EnumCollection;
using System.Collections;
using UnityEngine;
using PeggleMana;
using PeggleWars.Audio;

namespace PeggleOrbs
{
    public class Orb : MonoBehaviour
    {
        #region Fields

        [SerializeField] protected Mana _orbMana;
        [SerializeField] protected ManaType SpawnManaType;
        [SerializeField] protected int ManaAmount = 10;
        [SerializeField] protected Orb _defaultOrb;
        protected GameObject _spawnPoint;

        protected ManaPoolManager _manaPoolManager;

        protected Vector3 _position;

        #endregion

        #region Properties

        [SerializeField] protected OrbType _orbType;

        public OrbType OrbType
        {
            get { return _orbType; }
            private set { _orbType = value; }
        }

        #endregion

        #region Public Functions

        public virtual IEnumerator OrbEffect()
        {
            yield return null;
        }

        #endregion

        #region Protected Virtual Functions

        protected virtual void Start()
        {
            Physics2D.IgnoreLayerCollision(6, 7);
       
            _manaPoolManager = ManaPoolManager.Instance;
            _spawnPoint = FindSpawnPoint();
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains("Shot"))
            {
                AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX.SFX_0002_BasicPeggleHit);
                gameObject.GetComponent<SpriteRenderer>().size += new Vector2(0.03f, 0.03f);

                Orb orb = Instantiate(_defaultOrb, transform.position, Quaternion.identity);
                orb.gameObject.SetActive(false);

                SpawnMana();
                OrbManager.Instance.SceneOrbList.Remove(this);
                OrbManager.Instance.SceneOrbList.Add(orb);
                AdditionalEffects();

                StartCoroutine(DestroyOrb());
            }
        }

        protected virtual void AdditionalEffects()
        {
            //Add necessary additional effects in children here
        }

        //spawns the mana in the respective container
        protected virtual void SpawnMana()
        {
            Vector2 _spawnPointPosition = _spawnPoint.transform.position;
      
            for (int i = 0; i < ManaAmount; i++)
            {
                float _spawnRandomiserX = Random.Range(-0.7f, 0.7f);
                float _spawnRandomiserY = Random.Range(-0.2f, 0.2f);

                Vector2 _spawnPosition = new(_spawnPointPosition.x + _spawnRandomiserX, _spawnPointPosition.y + _spawnRandomiserY);

                Mana tempMana = Instantiate(_orbMana, _spawnPosition, Quaternion.identity);

                switch (SpawnManaType)
                {
                    case ManaType.BaseMana:
                        _manaPoolManager.BasicMana.Add(tempMana);
                        break;

                    case ManaType.FireMana:
                        _manaPoolManager.FireMana.Add(tempMana);
                        break;

                    case ManaType.IceMana:
                        _manaPoolManager.IceMana.Add(tempMana);
                        break;

                    case ManaType.LightningMana:
                        _manaPoolManager.LightningMana.Add(tempMana);
                        break;

                    case ManaType.DarkMana:
                        _manaPoolManager.DarkMana.Add(tempMana);
                        break;

                    case ManaType.LightMana:
                        _manaPoolManager.LightMana.Add(tempMana);
                        break;
                }
            }
        }

        #endregion

        #region Private Funtions

        private GameObject FindSpawnPoint()
        {
            GameObject spawnPoint = null;

            switch (SpawnManaType)
            {
                case ManaType.BaseMana:
                    spawnPoint = GameObject.FindGameObjectWithTag("BaseManaSpawn");
                    break;

                case ManaType.FireMana:
                    spawnPoint = GameObject.FindGameObjectWithTag("FireManaSpawn");
                    break;

                case ManaType.IceMana:
                    spawnPoint = GameObject.FindGameObjectWithTag("IceManaSpawn");
                    break;

                case ManaType.LightningMana:
                    spawnPoint = GameObject.FindGameObjectWithTag("LightningManaSpawn");
                    break;

                case ManaType.DarkMana:
                    spawnPoint = GameObject.FindGameObjectWithTag("DarkManaSpawn");
                    break;

                case ManaType.LightMana:
                    spawnPoint = GameObject.FindGameObjectWithTag("LightManaSpawn");
                    break;
            }

            return spawnPoint;
        }

        #endregion

        #region IEnumerators

        //Delays the "despawn" so that the size increase can be visible
        public IEnumerator SetInactive()
        {
            yield return new WaitForSeconds(0.1f);
            gameObject.GetComponent<SpriteRenderer>().size -= new Vector2(0.02f, 0.02f);
            gameObject.SetActive(false);
        }

        private IEnumerator DestroyOrb()
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }

        #endregion

    }
}

