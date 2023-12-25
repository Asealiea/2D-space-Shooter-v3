﻿using System.Collections;
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
    [SerializeField] private BoolSignal magnetSignal;
    
    [Space]
    
    [SerializeField] private bool hasShield; //if we have a shield or not

    private bool _hasTripleShot; //checks if we have triple shot power up
    private bool _hasSpeed; //checks to see if we have speed power up
    private int _tripleShotCount; // amount of triple shot power ups we have
    private int _speedUpCount; //amount of speed power ups we have
    private float _speedPowerUp = 1; // speed boost from speed power up
    private WaitForSeconds _coolDown; //cooldown for power ups
    //[SerializeField] private GameObject _secondaryFire; //not needed?
    private bool _hasSecondaryFire;
    //[SerializeField] private GameObject _homingMissile;
    private float _canMissile = -1f;

    [Header("Laser Settings")]
    [SerializeField] private float _fireRate = 0.3f;// how fast we can fire
    private float _canFire = -1f; //delay before firing again
    private readonly Vector3 _laserOffSet = new Vector3(0, 1.2f, 0);

    [Header("Health")]
    [SerializeField] private IntSignal playerLives;

    [Header("References")]
    [SerializeField] private GameObject _laserPreFab;

    [SerializeField] private GameObject _tripleShotPreFab;
    [SerializeField] private SpawnManager _spawnManager;

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
    [SerializeField] private FloatSignal thrusterSignal;


    //repair system
    //[Header("Thrusters")]
    private bool _repairing;
    private Coroutine RepairingIE;
    private float _extraWait = 3;
    private GameObject obj;


    private void OnEnable()
    {
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

        if (Input.GetKeyDown(KeyCode.C) && !magnetSignal.Value && !_repairing)
        {
            StartCoroutine(PowerUpMagnet());
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
                thrusterSignal.Decrement(0.25f);
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
        {
            obj = ObjectPool.SharedInstance.GetPooledObject("TripleShotAttack");
            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.identity;
            obj.SetActive(true);
        }
        else if (_hasSecondaryFire)
        {
            obj = ObjectPool.SharedInstance.GetPooledObject("SuperAttack");
            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.identity;
            obj.SetActive(true);
            ammoCount.Increment(1);
        }
        else 
        {
            obj = ObjectPool.SharedInstance.GetPooledObject("Laser");
            obj.transform.position = transform.position + _laserOffSet;
            obj.transform.rotation = Quaternion.identity;
            obj.SetActive(true);
        }
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
    }

    #region PowerUps

    #region missiles

    private void MissileFire()
    {
        _canMissile = Time.time + 2f;
        obj = ObjectPool.SharedInstance.GetPooledObject("MissileAttack");
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
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

    private IEnumerator SecondaryFireCoolDown()
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

    private IEnumerator TripleShotCoolDown()
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

    private IEnumerator SpeedCoolDown()
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

    private IEnumerator NegativePowerUpCoolDown()
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

    private IEnumerator PowerUpMagnet()
    {
        magnetSignal.SetValue(true);
        PowerUps[] powerUpsToGet = FindObjectsOfType<PowerUps>();
        if (powerUpsToGet.Length == 0)
        {
            StartCoroutine(MagnetCoolDown(2f));
            yield break;
        }

        foreach (var powerUp in powerUpsToGet)
        {
            powerUp.Magnet(this.transform);
        }
        StartCoroutine(MagnetCoolDown(5f));
    }

    private IEnumerator MagnetCoolDown(float time)
    {
        MineDisable();
        yield return new WaitForSeconds(time);
        magnetSignal.SetValue(false);
    }

    private void MineDisable()
    {
        Mine[] homingMines = FindObjectsOfType<Mine>();
        if (homingMines.Length == 0)
            return;
                
        foreach (var hMine in homingMines)
        {
            hMine.DestroyMine();
        }
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("EnemyLaser")|| !other.CompareTag("Mine")) return;
        
        Damage();
        ObjectPool.BackToPool(other.gameObject);
    }

    private void NullChecks()
    {
        if (_spawnManager == null)
            Debug.LogError("Player: SpawnManager is null");
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

    private IEnumerator Repearing()
    {
        yield return new WaitForSeconds(_extraWait);   
        lifeSignal.SetValue(true);
        ExtraLife();
        _repairing = false;
    }

    #endregion



}


