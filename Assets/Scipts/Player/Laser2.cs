using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser2 : MonoBehaviour
{
    private float _fireSpeed = 20f;
 


    void Update()
    {
        transform.Translate(Vector3.up * _fireSpeed * Time.deltaTime);
        if (transform.position.y >= 10)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
