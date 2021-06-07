using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //for player movement
    private float _multiplier = 5f;
    private float _shiftSpeed = 1f;
    //powerUps
    private bool _hasTripleShot;
    private bool _hasSpeed;
    private int _tripleShotCount;
    private int _speedUpCount;
    private float _speedPowerUp = 1;
    private WaitForSeconds _coolDown;
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
    [SerializeField] private GameObject _playerDeath;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private GameObject _thrustersRight, _thrustersLeft;



    void Start()
    {
        // take current pos = new pos(0,0,0)
        transform.position = Vector3.zero;
        NullChecks();
        //wait for seconds 5f
        _coolDown = new WaitForSeconds(5f);

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.Log("Player: AudioSource is null");
        else
            _audioSource.clip = _laserSound;
    }
   
    void Update()
    {
        PlayerMovement();   

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)    //_canFire == true
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ThrustersOn();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ThrustersOff();
        }
   
    }

    private void ThrustersOn()
    {
            _shiftSpeed = 1.5f;
            _thrustersLeft.transform.localScale = new Vector3(0.3f, 1, 1);
            _thrustersLeft.transform.position = transform.position + new Vector3(-0.25f, -1.592f, 0);
            _thrustersRight.transform.localScale = new Vector3(0.3f, 1, 1);
            _thrustersRight.transform.position = transform.position + new Vector3(0.25f, -1.592f, 0); 
    }
    private void ThrustersOff()
    { 
            _shiftSpeed = 1;
            _thrustersLeft.transform.localScale = new Vector3(0.15f, .5f, 0);
            _thrustersLeft.transform.position = transform.position + new Vector3(-0.25f, -1.22f, 0f);
            _thrustersRight.transform.localScale = new Vector3(0.15f, .5f, 0);
            _thrustersRight.transform.position = transform.position + new Vector3(0.25f, -1.22f, 0f);        
    }

    private void PlayerMovement()
    {
        float _horizontalInput = Input.GetAxisRaw("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");

        // transform.Translate(Vector3.right * _horizontalInput * (_multiplier +_speedPowerUp) * Time.deltaTime);
        // transform.Translate(Vector3.up * _verticalInput * (_multiplier +_speedPowerUp) * Time.deltaTime);

        Vector3 Direction = new Vector3(_horizontalInput, _verticalInput, 0);
        transform.Translate(Direction * (_multiplier* _shiftSpeed * _speedPowerUp) * Time.deltaTime);

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
        _audioSource.Play(0);//play the audio clip.
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
                Instantiate(_playerDeath, transform.position, Quaternion.identity);
                //instaniate the death explosion
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
        _tripleShotCount ++;
      
        if (!_hasTripleShot)
        {
            _hasTripleShot = true;
            StartCoroutine(TripleShotCoolDown());
        }
    }
    
    IEnumerator TripleShotCoolDown()
    {
        while (_tripleShotCount >=0)
        {
            yield return new WaitForSeconds(3f);
            _tripleShotCount--;
        }
        _hasTripleShot = false;
    }
    
    public void SpeedActive()
    {
        _speedPowerUp = 2f;
        _speedUpCount++;
        if (!_hasSpeed)
        {
            _hasSpeed = true;
            StartCoroutine(SpeedCoolDown());
        }
    }

    IEnumerator SpeedCoolDown()
    {
        while (_speedUpCount >=0)
        {
            yield return _coolDown;
            _speedUpCount--;
        }
       
        _speedPowerUp = 1;
        _hasSpeed = false;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ELaser"))
        {
            Damage();
        }
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
        if (_laserSound == null)
            Debug.Log("Player: Laser audio is null;");
        
  
    }

}
