using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _fireSpeed;

    // Start is called before the first frame update
    void Start()
    {
       // transform.Translate(Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _fireSpeed * Time.deltaTime);
        if (transform.position.y >= 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
