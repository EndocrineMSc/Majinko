using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars;
using EnumCollection;
using Player;
using System;

public class CloakedZombie : MonoBehaviour
{
    #region Fields

    [SerializeField] private float health = 20;
    private bool zombieDead;

    #endregion

    #region Properties

    [SerializeField] private int _damage;

    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    #endregion

    #region Private Functions

    private void Awake()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Spawn");
    }

    private void Update()
    {
        if (health <= 0 && !zombieDead)
        {
            StartCoroutine(nameof(ZombieDeath));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Bat"))
        {
            VampireBat _vampireBat = collision.gameObject.GetComponent<VampireBat>();
            health -= _vampireBat.Damage;
            gameObject.GetComponent<Animator>().SetTrigger("Hurt");
            Destroy(collision.gameObject);
        }
    }

    #endregion

    #region IEnumerators

    private IEnumerator ZombieDeath()
    {
        zombieDead = true;
        gameObject.GetComponent<Animator>().SetTrigger("Death");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    #endregion
}
