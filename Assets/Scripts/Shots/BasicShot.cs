using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicShot : MonoBehaviour
{
    #region Fields

    private Rigidbody2D _rigidbody;
    private bool _unshotBall = true;
    [SerializeField] private float force;

    //fields for Rotation

    private Camera _mainCam;
    private Vector3 _mousePosition;

    #endregion

    #region Properties

    private bool _ballHasStopped;

    public bool BallHasStopped
    {
        get { return _ballHasStopped; }
        private set { _ballHasStopped = value; }
    }


    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        //holds the ball in place until shot per mouseclidc
        _rigidbody.gravityScale = 0;

        _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }

    private void Update()
    {
        //following four lines set rotation around the ball, depending on mouse position
        _mousePosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 _direction = _mousePosition - transform.position;
        Vector3 _rotationShot = transform.position - _mousePosition;

        float _rotationZ = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;


        if (_unshotBall)
        {
            transform.rotation = Quaternion.Euler(0, 0, _rotationZ);
        }

        if (Input.GetMouseButtonDown(0) && _unshotBall)
        {
            //destroys the aiming arrow
            Destroy(transform.GetChild(0).gameObject);
            _rigidbody.gravityScale = 1;
            _rigidbody.velocity = new Vector2(_direction.x, _direction.y).normalized * force;

            _unshotBall = false;
        }
    }

    private void FixedUpdate()
    {
        StartCoroutine(nameof(CheckForStoppedBall));
    }

    //Makes the ball dissappear if it stops for 2 seconds
    private IEnumerator CheckForStoppedBall()
    {
        if (_unshotBall)
        {
            yield break;
        }

        if (Mathf.Abs(_rigidbody.velocity.x) <= 0.05 && Mathf.Abs(_rigidbody.velocity.y) <= 0.05)
        {
            yield return new WaitForSeconds(2f);

            if (Mathf.Abs(_rigidbody.velocity.x) <= 0.05 && Mathf.Abs(_rigidbody.velocity.y) <= 0.05)
            {
                _ballHasStopped = true;
            }
        }
    }
}
