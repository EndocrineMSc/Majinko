using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    /*private Camera _mainCam;
    private Vector3 _mousePosition;


    private void Start()
    {
        _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //following four lines set rotation around the ball, depending on mouse position
        _mousePosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 _rotation = _mousePosition - transform.position;

        float _rotationZ = Mathf.Atan2(_rotation.y, _rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, _rotationZ);

    }*/
}
