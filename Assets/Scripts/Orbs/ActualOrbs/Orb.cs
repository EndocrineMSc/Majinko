using EnumCollection;
using System.Collections;
using UnityEngine;
using PeggleWars.ManaManagement;
using PeggleWars.Audio;
using PeggleWars.ScrollDisplay;
using PeggleWars.Spheres;

namespace PeggleWars.Orbs
{
    [RequireComponent(typeof(ScrollDisplayer))]
    internal abstract class Orb : MonoBehaviour, IHaveDisplayDescription
    {
        #region Fields

        [SerializeField] protected Mana _orbMana;
        [SerializeField] protected ManaType _spawnManaType;
        [SerializeField] protected int _manaAmount = 10;
        [SerializeField] protected GameObject _defaultOrb;

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
            StartCoroutine(EnableCollider());
            SetScrollDisplayScale();
        }

        protected void SetScrollDisplayScale()
        {
            GetComponent<ScrollDisplayer>().DisplayScale = 4;
        }

        protected virtual IEnumerator EnableCollider()
        {
            yield return new WaitForSeconds(0.1f);
            GetComponent<Collider2D>().enabled = true;
        }

        protected virtual void SetReferences()
        {
            _manaPool = ManaPool.Instance;
            _orbManager = OrbManager.Instance;
            _audioManager = AudioManager.Instance;
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<IAmSphere>(out _))
            {
                GetComponent<Collider2D>().enabled = false;
                AdditionalEffectsOnCollision();
                PlayOrbOnHitSound();
                OnCollisionVisualPolish();
                SpawnMana();
                ReplaceHitOrb();               
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
            OrbEvents.Instance.ManaSpawnTrigger?.Invoke(_spawnManaType, _manaAmount);
        }

        protected virtual void ReplaceHitOrb()
        {
            GameObject orb = Instantiate(_defaultOrb, transform.position, Quaternion.identity);
            orb.SetActive(false);
            _orbManager.SceneOrbList.Remove(this);
            _orbManager.SceneOrbList.Add(orb.GetComponent<Orb>());
        }

        protected virtual IEnumerator DestroyOrb()
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }

        protected abstract void AdditionalEffectsOnCollision();

        public abstract void SetDisplayDescription();

        internal abstract IEnumerator OrbEffect();

        #endregion
    }
}

