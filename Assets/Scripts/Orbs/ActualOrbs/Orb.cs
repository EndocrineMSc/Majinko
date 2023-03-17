using EnumCollection;
using System.Collections;
using UnityEngine;
using PeggleWars.ManaManagement;
using PeggleWars.Audio;
using PeggleWars.ScrollDisplay;

namespace PeggleWars.Orbs
{
    [RequireComponent(typeof(ScrollDisplayer))]
    internal abstract class Orb : MonoBehaviour, IHaveDisplayDescription
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

        internal OrbType OrbType
        {
            get { return _orbType; }
            private set { _orbType = value; }
        }

        #endregion

        #region Functions

        internal abstract IEnumerator OrbEffect();

        //Delays the "despawn" so that the size increase can be visible
        internal IEnumerator SetInactive()
        {
            yield return new WaitForSeconds(0.15f);
            gameObject.GetComponent<SpriteRenderer>().size -= new Vector2(0.02f, 0.02f);
            gameObject.SetActive(false);
        }

        private void Start()
        {
            SetReferences();
            SetDisplayDescription();
        }

        protected virtual void SetReferences()
        {
            _manaPool = ManaPool.Instance;
            _orbManager = OrbManager.Instance;
            _audioManager = AudioManager.Instance;
            _spawnPoint = FindSpawnPoint();
        }

        private GameObject FindSpawnPoint()
        {
            GameObject spawnPoint = null;

            switch (SpawnManaType)
            {
                case ManaType.BasicMana:
                    spawnPoint = GameObject.FindGameObjectWithTag("BaseManaSpawn");
                    break;

                case ManaType.FireMana:
                    spawnPoint = GameObject.FindGameObjectWithTag("FireManaSpawn");
                    break;

                case ManaType.IceMana:
                    spawnPoint = GameObject.FindGameObjectWithTag("IceManaSpawn");
                    break;
            }

            return spawnPoint;
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

        protected virtual void PlayOrbOnHitSound()
        {
            _audioManager.PlaySoundEffectWithoutLimit(SFX._0002_BasicPeggleHit);
        }

        protected virtual void OnCollisionVisualPolish()
        {
            gameObject.GetComponent<SpriteRenderer>().size += new Vector2(0.03f, 0.03f);
        }

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
                    case ManaType.BasicMana:
                        _manaPool.BasicMana.Add(tempMana);
                        break;

                    case ManaType.FireMana:
                        _manaPool.FireMana.Add(tempMana);
                        break;

                    case ManaType.IceMana:
                        _manaPool.IceMana.Add(tempMana);
                        break;
                }
            }
        }

        protected virtual void ReplaceHitOrb()
        {
            GameObject orb = Instantiate(_defaultOrb, transform.position, Quaternion.identity);
            orb.SetActive(false);
            _orbManager.SceneOrbList.Remove(this);
            _orbManager.SceneOrbList.Add(orb.GetComponent<Orb>());
        }

        protected virtual void AdditionalEffectsOnCollision()
        {
            //Add necessary additional effects in children here
        }

        private IEnumerator DestroyOrb()
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }

        public abstract void SetDisplayDescription();

        #endregion
    }
}

