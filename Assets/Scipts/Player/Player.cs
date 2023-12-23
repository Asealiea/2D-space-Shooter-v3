using System;
using System.Collections;
using System.Collections.Generic;
using RSG.Trellis.Signals;
using UnityEngine;

public class Player : MonoBehaviour
{
    //for player movement
    [Header("Player movement")]
    [SerializeField] private float shiftSpeedBoost = 1.75f; //speed boost from shift
    //[SerializeField] private float _thrusters = 100f; // used to update the thruster bar
    private float _negative = 1f;
    private const float Multiplier = 5f; //basic movement
    private float _shiftSpeed = 1f; //multiplier that changes if shift is held
    private float _extraBoosts;


    [Header("Power Ups")]
    [Space]
    
    [SerializeField] private BoolSignal tripleShotSignal;
    [SerializeField] private BoolSignal speedSignal;
    [SerializeField] private BoolSignal shieldsSignal;
    [SerializeField] private BoolSignal ammoSignal;
    [SerializeField] private IntSignal ammoCount;
    [SerializeField] private BoolSignal lifeSignal;
    [SerializeField] private BoolSignal missileSignal;
    [SerializeField] private IntSignal missileCount;
    [SerializeField] private BoolSignal negativeSignal;
    [SerializeField] private BoolSignal superSignal;
    
    [Space]
    
    [SerializeField] private bool hasShield; //if we have a shield or not

    private bool _hasTripleShot; //checks if we have triple shot power up
    private bool _hasSpeed; //checks to see if we have speed power up
    private int _tripleShotCount; // amount of triple shot power ups we have
    private int _speedUpCount; //amount of speed power ups we have
    private float _speedPowerUp = 1; // speed boost from speed power up
    private WaitForSeconds _coolDown; //cooldown for power ups
    [SerializeField] private GameObject _secondaryFire;
    private bool _hasSecondaryFire;
    [SerializeField] private GameObject _homingMissile;
    private float _canMissile = -1f;
    //[SerializeField] private int _missileCount = 3;

    [SerializeField] private bool _magnet = false;

    [Header("Laser Settings")]
    private float _canFire = -1f; //delay before firing again

    [SerializeField] private float _fireRate = 0.3f;// how fast we can fire

    private readonly Vector3 _laserOffSet = new Vector3(0, 1.2f, 0);
    //[SerializeField] private int _ammoCount = 15;


    [Header("Health")]
    [SerializeField] private IntSignal playerLives;

    [Header("References")]
    [SerializeField] private GameObject _laserPreFab;

    [SerializeField] private GameObject _tripleShotPreFab;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private GameObject _playerDamageLeft, _playerDamageRight;
    [SerializeField] private GameObject _playerDeath;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private ShieldSignal _shield;
    [SerializeField] private AudioClip _ammoEmptyClip;
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private Animator _anim;


    //thrusters
    [Header("Thrusters")]
    [SerializeField] private BoolSignal thrusting;
    [SerializeField] private BoolSignal thrustersUsable;
    [SerializeField] private IntSignal thrusterSignal;


    //repair system
    //[Header("Thrusters")]
    private bool _repairing;
    private Coroutine RepairingIE;
    private float _extraWait = 3;


    private void OnEnable()
    {
        playerLives.AddListener(UpdateLives);
        tripleShotSignal.AddListener(TripleShotActive);
        speedSignal.AddListener(SpeedActive);
        shieldsSignal.AddListener(ShieldsActive);
        ammoSignal.AddListener(ExtraAmmo);
        lifeSignal.AddListener(ExtraLife);
        missileSignal.AddListener(MissilePayload);
        negativeSignal.AddListener(NegativePowerUp);
        superSignal.AddListener(SecondaryFire);
    }

    private void OnDisable()
    {
        playerLives.RemoveListener(UpdateLives);
        tripleShotSignal.RemoveListener(TripleShotActive);
        speedSignal.RemoveListener(SpeedActive);
        shieldsSignal.RemoveListener(ShieldsActive);
        ammoSignal.RemoveListener(ExtraAmmo);
        lifeSignal.RemoveListener(ExtraLife);
        missileSignal.RemoveListener(MissilePayload);
        negativeSignal.RemoveListener(NegativePowerUp);
        superSignal.RemoveListener(SecondaryFire);
    }


    void Start()
    {
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        transform.position = Vector3.zero;
        NullChecks();
        _coolDown = new WaitForSeconds(5f);
        
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.Log("Player: AudioSource is null");
        else
            _audioSource.clip = _laserSound;
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!_repairing)
            PlayerMovement();

        if (Input.GetKeyDown(KeyCode.C) && !_magnet && !_repairing)
        {
            StartCoroutine(PowerupMagnet());
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && ammoCount.Value > 0 && !_repairing)
        {
            Shoot();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && ammoCount.Value <= 0)
        {
            _audioSource.clip = _ammoEmptyClip;
            _audioSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time > _canMissile && missileCount.Value > 0 && !_repairing)
        {
            MissileFire();
        }
                
        if (Input.GetKeyDown(KeyCode.R))
        {
            // repair ship but can't move
            if (playerLives.Value < 3 && !_repairing)
            {
                _repairing = true;                
                RepairingIE = StartCoroutine(Repearing());
            }
        }

        if (Input.GetKey(KeyCode.LeftShift) && thrustersUsable.Value && !_repairing)
        {
            if (thrusterSignal.Value > 0)
            {
                thrusting.SetValue(true);
                _shiftSpeed = shiftSpeedBoost;
                thrusterSignal.Increment(-1);
            }

            if (thrusterSignal.Value <= 0)
            {
                thrusting.SetValue(false);
                thrustersUsable.SetValue(false);
                thrusterSignal.SetValue(0);
            }
        }
        else
        {
            _shiftSpeed = 1;
        }
        

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _shiftSpeed = 1;
            thrusting.SetValue(false);
        }
    }

    #region Movement

    private void PlayerMovement()
    {
        float _horizontalInput = Input.GetAxisRaw("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");
        _anim.SetFloat("Turning", _horizontalInput);
        var bounds = Vector3.zero;

        // transform.Translate(Vector3.right * _horizontalInput * (_multiplier +_speedPowerUp) * Time.deltaTime);
        // transform.Translate(Vector3.up * _verticalInput * (_multiplier +_speedPowerUp) * Time.deltaTime);

        Vector3 direction = new Vector3(_horizontalInput, _verticalInput, 0);
        _extraBoosts = (Multiplier * _shiftSpeed * _negative * _speedPowerUp) * Time.deltaTime;
        transform.Translate(direction * _extraBoosts);

        if (transform.position.y >= 0)
        {
            bounds.y = 0;
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            bounds.y = -3.8f;
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }
        if (transform.position.x <= -11.3f)
        {
            bounds.x = 11.3f;
            transform.position = new Vector3(11.3f,transform.position.y, 0);
        }
        else if (transform.position.x >= 11.3f)
        {
            bounds.x = -11.3f;
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }

      //  transform.position = bounds;

    }

    #endregion

    private void Shoot()
    {
        _canFire = Time.time + _fireRate;
        _audioSource.clip = _laserSound;
        if (_hasTripleShot)
            Instantiate(_tripleShotPreFab, transform.position, Quaternion.identity);
        else if (_hasSecondaryFire)
        {
            Instantiate(_secondaryFire, transform.position, Quaternion.identity);
            ammoCount.Increment(1);
        }
        else
            Instantiate(_laserPreFab, transform.position + _laserOffSet, Quaternion.identity);          
        _audioSource.Play(0);//play the audio clip.
        ammoCount.Increment(-1);
    }

    public void Damage()
    {
        if (_repairing)
        {
            StopCoroutine(RepairingIE);
            _repairing = false;
        }

        //TODO
        if (hasShield)
        {
            switch (_shield.Value)
            {
                case 1: // last hit, remove shields
                    _shield.SetValue(0);
                    hasShield = false;
                    break;
                case 2:// taken 1 hit
                    _shield.SetValue(1);
                    break;
                case 3: //have fresh shield
                    _shield.SetValue(2);
                    break;
            }
            return;
        }
        playerLives.Increment(-1);

        switch (playerLives.Value)
        {
            case 1:
                _cameraShake.CameraShaker(2);
                break;
            case 2:
                _cameraShake.CameraShaker(1);
                break;
            default:
                break;
        }
      
    }

    private void UpdateLives()
    {
        switch (playerLives.Value)
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
    }

    #region PowerUps

    #region missiles

    private void MissileFire()
    {
        _canMissile = Time.time + 2f;
        Instantiate(_homingMissile, transform.position, Quaternion.identity);
        missileCount.Increment(-1);
    }

    private void MissilePayload()
    {
        if (!missileSignal.Value) return;

        if (missileCount.Value < 5)
        {          
            missileCount.Increment(1);
        }
        missileSignal.SetValue(false);
    }

    #endregion

    #region SecondaryFire

    private void SecondaryFire()
    {
        if (!superSignal.Value) return;
        if (_hasSecondaryFire) return;
        
        _hasTripleShot = false;
        _hasSecondaryFire = true;
        _fireRate = 0.75f;
        StartCoroutine(SecondaryFireCoolDown());
    }
    
    IEnumerator SecondaryFireCoolDown()
    {
        yield return new WaitForSeconds(5f);
        _hasSecondaryFire = false;  
        superSignal.SetValue(false);
        _fireRate = 0.3f;
    }
    #endregion

                        
    #region TripleShot

    private void TripleShotActive()
    {
        if (!tripleShotSignal.Value) return;

        if (_hasSecondaryFire != true)
        {
            _tripleShotCount ++;
      
            if (!_hasTripleShot)
            {
                _hasTripleShot = true;
                StartCoroutine(TripleShotCoolDown());
            }
            tripleShotSignal.SetValue(false);
        }
        else
        {
            tripleShotSignal.SetValue(false);
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

    #endregion

    #region SpeedBoost

    private void SpeedActive()
    {
        if (!speedSignal.Value) return;

        _speedPowerUp = 2f;
        _speedUpCount++;
        if (!_hasSpeed)
        {
            _hasSpeed = true;
            StartCoroutine(SpeedCoolDown());
        }
        speedSignal.SetValue(false);
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

    #endregion

    private void ShieldsActive()
    {
        if (!shieldsSignal.Value) return;
        hasShield = true; 
        _shield.SetValue(3);
        shieldsSignal.SetValue(false);
    }

    #region NegativePowerup

    private void NegativePowerUp()
    {
        if (!negativeSignal.Value) return;
        hasShield = false;
        _shield.SetValue(0);
        Damage();
        _negative = 0.3f;
        StartCoroutine(NegativePowerUpCoolDown());
    }

    IEnumerator NegativePowerUpCoolDown()
    {
        yield return new WaitForSeconds(5f);
        _negative = 1f;
        negativeSignal.SetValue(false);
    }

    #endregion

    #endregion

    #region Picksups

    private void ExtraAmmo()
    {
        if (!ammoSignal.Value) return;

        ammoCount.Increment(10);
        if (ammoCount.Value >= 30)
        {
            ammoCount.SetValue(30);
        }
        ammoSignal.SetValue(false);
    }

    private void ExtraLife()
    {
        if (!lifeSignal.Value) return;
        
        if (playerLives.Value < 3)
        {
            playerLives.Increment(1);
        }
        lifeSignal.SetValue(false);
    }

    IEnumerator PowerupMagnet()
    {
        _magnet = true;
        GameObject[] PowerupsToGet = GameObject.FindGameObjectsWithTag("Powerup");
        if (PowerupsToGet.Length == 0)
        {
            _uiManager.MagnetOff();
            StartCoroutine(MagnetCoolDown(2f));
            yield break;
        }

        foreach (var powerup in PowerupsToGet)
        {
            PowerUps powerupScript = powerup.GetComponent<PowerUps>();
            powerupScript.Magnet(this.transform);
        }
        _uiManager.MagnetOff();
        StartCoroutine(MagnetCoolDown(5f));
        yield break;
    }

    IEnumerator MagnetCoolDown(float time)
    {
        MineDisable();
        yield return new WaitForSeconds(time);
        _magnet = false;
        _uiManager.MagnetOn();
        yield break;
    }

    public void MineDisable()
    {
        GameObject[] homingMines = GameObject.FindGameObjectsWithTag("Mine");
        if (homingMines.Length == 0)
            return;
                
        foreach (var hMine in homingMines)
        { 
            Mine mine = hMine.GetComponentInChildren<Mine>();
            if (mine != null)
                mine.DestroyMine();                       
        }
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("ELaser")) return;
        
        Damage();
        Destroy(other.gameObject);
    }

    private void NullChecks()
    {
        if (_spawnManager == null)
            Debug.LogError("Player: SpawnManager is null");
        if (_uiManager == null)
            Debug.LogError("Player: UI Manager is null");
        if (_tripleShotPreFab == null)
            Debug.LogError("Player: TripleShot is null");
        if (_laserPreFab == null)
            Debug.LogError("Player: Laser is null");
        if (_laserSound == null)
            Debug.Log("Player: Laser audio is null;");
        if (_cameraShake == null)
            Debug.LogError("Player: Camera Shaker is null");
        if (_anim == null)
            Debug.LogError("Player:Anim is null");
    }

    #region Repair

    public void LongerWaitTime(int timer)
    {
        _extraWait = timer;
    }

    IEnumerator Repearing()
    {
        yield return new WaitForSeconds(_extraWait);   
        lifeSignal.SetValue(true);
        ExtraLife();
        _repairing = false;
    }

    #endregion



}


