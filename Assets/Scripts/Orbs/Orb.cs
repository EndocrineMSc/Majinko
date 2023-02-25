using EnumCollection;
using System.Collections;
using UnityEngine;
using PeggleWars.ManaManagement;
using PeggleWars.Audio;

namespace PeggleWars.Orbs
{
    /// <summary>
    /// Parent class to all orbs. Defines what a Orb is. BaseManaOrb directly uses this script. Every other orb needs a new class inheriting this one.
    /// </summary>
    public class Orb : MonoBehaviour
    {
        #region Fields

        [SerializeField] protected Mana _orbMana;
        [SerializeField] protected ManaType SpawnManaType;
        [SerializeField] protected int ManaAmount = 10;
        [SerializeField] protected GameObject _defaultOrb;
        protected GameObject _spawnPoint;

        protected ManaPool _manaPool;
        protected OrbManager _orbManager;
        protected AudioManager _audioManager;

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

        //Delays the "despawn" so that the size increase can be visible
        public IEnumerator SetInactive()
        {
            yield return new WaitForSeconds(0.15f);
            gameObject.GetComponent<SpriteRenderer>().size -= new Vector2(0.02f, 0.02f);
            gameObject.SetActive(false);
        }

        #endregion

        #region Private Functions

        private void Start()
        {
            SetReferences();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains("Shot"))
            {
                PlayOrbOnHitSound();
                OnCollisionVisualPolish();
                SpawnMana();
                ReplaceHitOrb();               
                AdditionalEffectsOnCollision();
                StartCoroutine(DestroyOrb());
            }
        }

        private IEnumerator DestroyOrb()
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }

        private GameObject FindSpawnPoint()
        {
            GameObject spawnPoint = null;

            switch (SpawnManaType)
            {
                case ManaType.BaseMana:
                    spawnPoint = GameObject.FindGameObjectWithTag("BaseManaSpawn");
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

        #region Protected Virtual Functions

        protected virtual void SetReferences()
        {
            _manaPool = ManaPool.Instance;
            _orbManager = OrbManager.Instance;
            _audioManager = AudioManager.Instance;
            _spawnPoint = FindSpawnPoint();
        }

        protected virtual void ReplaceHitOrb()
        {
            GameObject orb = Instantiate(_defaultOrb, transform.position, Quaternion.identity);
            orb.SetActive(false);
            _orbManager.SceneOrbList.Remove(this);
            _orbManager.SceneOrbList.Add(orb.GetComponent<Orb>());
        }

        protected virtual void PlayOrbOnHitSound()
        {
            _audioManager.PlaySoundEffectWithoutLimit(SFX._0002_BasicPeggleHit);
        }

        protected virtual void AdditionalEffectsOnCollision()
        {
            //Add necessary additional effects in children here
        }

        protected virtual void OnCollisionVisualPolish()
        {
            gameObject.GetComponent<SpriteRenderer>().size += new Vector2(0.03f, 0.03f);
        }

        //spawns the mana in the respective container
        protected virtual void SpawnMana()
        {
            Vector2 _spawnPointPosition = _spawnPoint.transform.position;

            for (int i = 0; i < ManaAmount; i++)
            {
                float _spawnRandomiserX = Random.Range(-0.2f, 0.2f);
                float _spawnRandomiserY = Random.Range(-0.2f, 0.2f);

                Vector2 _spawnPosition = new(_spawnPointPosition.x + _spawnRandomiserX, _spawnPointPosition.y + _spawnRandomiserY);

                Mana tempMana = Instantiate(_orbMana, _spawnPosition, Quaternion.identity);

                switch (SpawnManaType)
                {
                    case ManaType.BaseMana:
                        _manaPool.BasicMana.Add(tempMana);
                        break;

                    case ManaType.DarkMana:
                        _manaPool.DarkMana.Add(tempMana);
                        break;

                    case ManaType.LightMana:
                        _manaPool.LightMana.Add(tempMana);
                        break;
                }
            }
        }

        #endregion

    }
}

