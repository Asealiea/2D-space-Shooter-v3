using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _multiplier = 5f;


    [SerializeField] private GameObject _laserPreFab;


    // Start is called before the first frame update
    void Start()
    {
        // take current pos = new pos(0,0,0)
        transform.position = Vector3.zero;
      
    }

    // Update is called once per frame
    void Update()
    { 




  
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void PlayerMovement()
    {
        float _horizontalInput = Input.GetAxisRaw("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * _horizontalInput * _multiplier * Time.deltaTime);
        transform.Translate(Vector3.up * _verticalInput * _multiplier * Time.deltaTime);


        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }
        if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f,transform.position.y, 0);
        }
        else if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }

    }

    private void Shoot()
    {
        Instantiate(_laserPreFab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);

    }




}
