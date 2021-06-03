using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //for player movement
    private float _multiplier = 5f;
    //powerUps
    private bool _hasTripleShot;
    private float _speedPowerUp = 1;
    [SerializeField] private bool _hasShield;
    //score
    [SerializeField] private int _score;

    [Header("Laser Settings")]
    private float _canFire = -1f;
    [SerializeField] private float _fireRate = 0.3f;
   
    [Header("Health")]
    [SerializeField] private int _lives = 3;

    [Header("References")]
    [SerializeField] private GameObject _laserPreFab;
    [SerializeField] private GameObject _tripleShotPreFab;
    [SerializeField] private GameObject _shields;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private GameObject _playerDamageLeft, _playerDamageRight;



    void Start()
    {
        // take current pos = new pos(0,0,0)
        transform.position = Vector3.zero;
        NullChecks();

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

        // transform.Translate(Vector3.right * _horizontalInput * (_multiplier +_speedPowerUp) * Time.deltaTime);
        // transform.Translate(Vector3.up * _verticalInput * (_multiplier +_speedPowerUp) * Time.deltaTime);

        Vector3 Direction = new Vector3(_horizontalInput, _verticalInput, 0);
        transform.Translate(Direction * (_multiplier * _speedPowerUp) * Time.deltaTime);

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

        if (_hasTripleShot == true)
        {
            Instantiate(_tripleShotPreFab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPreFab, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        }   
    }

    public void Damage()
    {
        if (_hasShield == true)
        {
            _shields.SetActive(false);
            _hasShield = false;
            return;
        }
        _lives--;
        _uiManager.UpdateLives(_lives);



        switch (_lives)
        {
            case 0:
                _spawnManager.StopSpawning();
                _uiManager.GameOver();
                Destroy(this.gameObject);
                break;
            case 1:
                _playerDamageRight.SetActive(true);
                break;
            case 2:
                _playerDamageLeft.SetActive(true);
                _playerDamageRight.SetActive(false);
                break;

            default:
                _playerDamageRight.SetActive(false);
                _playerDamageLeft.SetActive(false);
                break;
        }

        /*
            if (_lives <= 0)
        {
            _spawnManager.StopSpawning();
            _uiManager.GameOver(); // call the game over
            //instantiate explosion 
            Destroy(this.gameObject);//add delay to destory
        }
        
            */



    }

    public void TripleShotActive()
    {
        _hasTripleShot = true;
        //obtained tripleshots + 1
        StartCoroutine(TripleShotCoolDown());
    }

    IEnumerator TripleShotCoolDown()
    {
        /*
         * while obtained tripleshots  is greater then 0
         * wait for seconds 
         * 
         * afterwards revert back to false.
          */
        yield return new WaitForSeconds(5f);
        _hasTripleShot = false;
    }

    public void SpeedActive()
    {
        //speed up player.
        _speedPowerUp = 2f;
        StartCoroutine(SpeedCoolDown());
    }

    IEnumerator SpeedCoolDown()
    {
        yield return new WaitForSeconds(5f);
        _speedPowerUp = 1f;
        //revert speed back to normal.
    }

    public void ShieldsActive()
    {
        _hasShield = true;
        _shields.SetActive(true);
        // deploy the shields.
    }

    public void AddScore(int addPoints)
    {
        _score += addPoints;
        _uiManager.UpdateScore(_score);
        
    }

    private void NullChecks()
    {
        if (_spawnManager == null)
            Debug.LogError("Player: SpawnManager is null");
        if (_uiManager == null)
            Debug.LogError("Player: UI Manager is null");
        if (_shields == null)
            Debug.LogError("Player: Shields is null");
        if (_tripleShotPreFab == null)
            Debug.LogError("Player: TripleShot is null");
        if (_laserPreFab == null)
            Debug.LogError("Player: Laser is null");
  
    }

}
