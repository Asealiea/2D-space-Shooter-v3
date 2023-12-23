using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser2 : MonoBehaviour
{
    private const float FireSpeed = 20f;
    private float _moveSpeed;

    private void OnEnable()
    {
        _moveSpeed = FireSpeed * Time.deltaTime;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * _moveSpeed);
        
        if (!(transform.position.y >= 10)) return;
        
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        Destroy(this.gameObject);
    }
}
