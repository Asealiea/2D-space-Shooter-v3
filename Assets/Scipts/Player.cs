using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //for player movement
    [Header("Player movement")]
    private float _multiplier = 5f; //basic movement
    private float _shiftSpeed = 1f; //multiplier that changes if shift is held
    [SerializeField] private float _shiftSpeedBoost = 1.75f; //speed boost from shift
    [SerializeField] private float _thrusters = 100f; // used to update the thruster bar
    [SerializeField] private bool _canThrust; //checks if the player can use the thrusters
    [SerializeField] private bool _thrusterCharging; //checks if the player is already charging thrusters
    
    [Header("Powerups")]
    private bool _hasTripleShot; //checks if we have triple shot power up
    private bool _hasSpeed; //checks to see if we have speed power up
    private int _tripleShotCount; // amount of triple shot power ups we have
    private int _speedUpCount; //amount of speed power ups we have
    private float _speedPowerUp = 1; // speed boost from speed power up
    private WaitForSeconds _coolDown; //cooldown for power ups
    [SerializeField] private bool _hasShield; //if we have a shield or not
    [SerializeField] private int _shieldCount; // count for sheilds

    [SerializeField] private GameObject _secondaryFire;
    [SerializeField] private bool _hasSecondaryFire;
   
    [Header("Score")]
    [SerializeField] private int _score; // players score

    [Header("Laser Settings")]
    private float _canFire = -1f; //delay before firing again
    [SerializeField] private float _fireRate = 0.3f;// how fast we can fire
    [SerializeField] private int _ammoCount = 15;
   
    [Header("Health")]
    [SerializeField] private int _lives = 3;//total lives

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
    [SerializeField] private Renderer _shieldsRenderer;
    [SerializeField] private AudioClip _ammoEmptyClip;
    [SerializeField] private CameraShake _cameraShake;



    void Start()
    {
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        _shieldsRenderer = _shields.GetComponent<Renderer>();
        transform.position = Vector3.zero;
        NullChecks();
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

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCount > 0)    
            Shoot();
        else if (Input.GetKeyDown(KeyCode.Space) && _ammoCount <= 0)
        {
            _audioSource.clip = _ammoEmptyClip;
            _audioSource.Play();
        }
        
        if (Input.GetKey(KeyCode.LeftShift) && _canThrust)
            ThrustersOn();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            ThrustersOff();
    }

    private void ThrustersOn()
    {
        _shiftSpeed = _shiftSpeedBoost;
        _thrustersLeft.transform.localScale = new Vector3(0.3f, 1, 1);
        _thrustersLeft.transform.position = transform.position + new Vector3(-0.25f, -1.592f, 0);
        _thrustersRight.transform.localScale = new Vector3(0.3f, 1, 1);
        _thrustersRight.transform.position = transform.position + new Vector3(0.25f, -1.592f, 0);
        if (_thrusters > 0)
        {
            _thrusters -= 1f;
            ThursterBar();
        }
    }
        
    private void ThrustersOff()
    {
        _shiftSpeed = 1;
        _thrustersLeft.transform.localScale = new Vector3(0.15f, .5f, 0);
        _thrustersLeft.transform.position = transform.position + new Vector3(-0.25f, -1.22f, 0f);
        _thrustersRight.transform.localScale = new Vector3(0.15f, .5f, 0);
        _thrustersRight.transform.position = transform.position + new Vector3(0.25f, -1.22f, 0f);
        if (_thrusters > 0.5f && !_thrusterCharging)
        {
            StartCoroutine(ThrusterRecharge());
        }
        if (_thrusters <= 0)
        {
            StartCoroutine(ThrustersCoolDown());
        }
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
        _audioSource.clip = _laserSound;
        if (_hasTripleShot == true)
            Instantiate(_tripleShotPreFab, transform.position, Quaternion.identity);
        else if (_hasSecondaryFire == true)
        {
            Instantiate(_secondaryFire, transform.position, Quaternion.identity);
            _ammoCount++;
        }
        else
            Instantiate(_laserPreFab, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);          
        _audioSource.Play(0);//play the audio clip.
        _ammoCount--;
        _uiManager.UpdateAmmo(_ammoCount);
    }
        
    public void Damage(bool Damage) // false = no damage, true = damage
    {
        if (Damage)
        {
            if (_hasShield == true)
            {
                switch (_shieldCount)
                {
                    case 1: // last hit, remove shields
                        _shieldsRenderer.material.color = Color.white;
                        _shields.SetActive(false);
                        _hasShield = false;
                        break;
                    case 2:// taken 1 hit
                        _shieldCount--;
                        _shieldsRenderer.material.color = Color.red;
                        break;
                    case 3: //have fresh shield
                        _shieldCount--;
                        _shieldsRenderer.material.color = Color.green;
                        break;
                }
                return;
            }
            _lives--;
        }
            _uiManager.UpdateLives(_lives);

            switch (_lives)
            {
                case 0:
                    _spawnManager.StopSpawning();
                    _uiManager.GameOver();
                    Instantiate(_playerDeath, transform.position, Quaternion.identity);
                    _cameraShake.CameraShaker(3);
                    //instaniate the death explosion
                    Destroy(this.gameObject);
                    break;
                case 1:
                    _playerDamageRight.SetActive(true);
                    if (Damage)
                        _cameraShake.CameraShaker(2);
                    break;
                case 2:
                    _playerDamageLeft.SetActive(true);
                    _playerDamageRight.SetActive(false);
                    if (Damage)
                        _cameraShake.CameraShaker(1);
                    break;
                default:
                    _playerDamageRight.SetActive(false);
                    _playerDamageLeft.SetActive(false);
                    break;
            }    
    }
    
    public void SecondaryFire()
    {
        _hasSecondaryFire = true;
        _fireRate = 0.75f;
        StartCoroutine(SecondaryFireCoolDown());
    }

    IEnumerator SecondaryFireCoolDown()
    {
        yield return new WaitForSeconds(5f);
        _hasSecondaryFire = false;
        _fireRate = 0.3f;
    }

    public void TripleShotActive()
    {
        if (_hasSecondaryFire != true)
        {
            _tripleShotCount ++;
      
            if (!_hasTripleShot)
            {
                _hasTripleShot = true;
                StartCoroutine(TripleShotCoolDown());
            }

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
        //_shieldsRenderer.material.color = Color.white; //change the colour to white
        _shieldCount = 3; // add 3 shields 
        _shields.SetActive(true); // deploy the shields.
    }

    public void ExtraAmmo()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void ExtraLife()
    {
        if (_lives < 3)
        {
            _lives++;
        }
        Damage(false); // false = no damage
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
            Damage(true);
            Destroy(other.gameObject);
        }
    }

    IEnumerator ThrustersCoolDown()
    {
        _canThrust = false;
        yield return new WaitForSeconds(5f);
        StartCoroutine(ThrusterRecharge());
    }

    IEnumerator ThrusterRecharge()
    {
        _thrusterCharging = true;
        while (_thrusters < 100)
        {
            yield return new WaitForSeconds(0.1f);
            _thrusters+= 1f;
            ThursterBar();
        }
        _canThrust = true;
        _thrusterCharging = false;

    }

    private void ThursterBar()
    {
        _uiManager.ThrusterUpdate(_thrusters);
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
        if (_cameraShake == null)
            Debug.LogError("Player: Camera Shaker is null");
    }


}


        