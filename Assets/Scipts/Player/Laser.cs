using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float fireSpeed;
    private float _moveSpeed;


    private void OnEnable()
    {
        _moveSpeed = fireSpeed * Time.deltaTime;
    }

    void Update()
    {
  
        transform.Translate(Vector3.up * _moveSpeed);
 
        
        if (transform.position.y >= 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            //pool instead of destroy
            Destroy(this.gameObject);
        }
        if (transform.position.y <= -5.5)
        {
            //pool instead of destroy
            Destroy(this.gameObject);
        }
    }

}
