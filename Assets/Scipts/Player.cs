using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Firing laser Settings")]
    private float _canFire = -1f;
    [SerializeField] private float _fireRate = 2f;
   
    [Header("Health")]
    [SerializeField] private int _lives = 3;



    //for player movement
    private float _multiplier = 5f; 
   
    [Header("Prefabs")]
    [SerializeField] private GameObject _laserPreFab;
    [SerializeField] private SpawnManager _spawnManager;


    


    void Start()
    {
        // take current pos = new pos(0,0,0)
        transform.position = Vector3.zero;
        if (_spawnManager == null)
        {
            Debug.LogError("Player: SpawnManager is null");
        }
      
    }

   
    void Update()
    {
        PlayerMovement();   

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)    //_canFire == true
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
        _canFire = Time.time + _fireRate;
        Instantiate(_laserPreFab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
         
    }

    public void Damage()
    {
        _lives--;
        if (_lives <= 0)
        {
            _spawnManager.StopSpawning();
            Destroy(this.gameObject);
        }
    }



}
