using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireBat : MonoBehaviour
{

    private float _damage;
    private Rigidbody2D _rigidbody;
    private float _spriteSize;
    [SerializeField, Range(5f, 10f)] private float _batSpeed;
    private SpriteRenderer _spriteRenderer;

    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.AddForce(Vector2.right * _batSpeed);
        _spriteSize = (_damage / 10);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.size += new Vector2(0.8f + _spriteSize, 0.8f + _spriteSize);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody.AddForce(Vector2.right * _batSpeed);
    }
}
