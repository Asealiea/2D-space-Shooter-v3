using System.Collections;
using System.Collections.Generic;
using RSG.Trellis.Signals;
using UnityEngine;

public class Boss : MonoBehaviour
{

    [SerializeField] private bool _leftGun, _rightGun = false; // well you saw these 2 in action already
    [SerializeField] private GameObject _charge, _laser; //new attack type when down to the body
    private float _canFire; //the time when the boss can attack 
    private float _fireRate; //how fast the boss can attack
    [SerializeField] private int _bossHealth; // how many hits the boss takes to fully die
    private int _death; // stages of death could have used a enum
    [SerializeField] private GameObject _explosion1, _explosion2, _explosion3, _explosion4; //lots of booms
    [SerializeField] private CameraShake _cameraShake; //getting shaky when the boss dies
    [SerializeField] private Transform _laserTransform; //used to shoot the lasers from, me being lazy.
    [SerializeField] private Animator _anim;//so we can stop moving when boss dies.
    [SerializeField] private int _bossID;
    [SerializeField] private WaitForSeconds _wait;
    [SerializeField] private GameObject _starfall;
    [SerializeField] private Transform _leftGunTrans, _rightGunTrans;
    [SerializeField] private FloatSignal shakeSignal;

    // Start is called before the first frame update
    void Start()
    {
        _canFire = 1; //gives it a second before it starts attacking
        _fireRate = 3; // makes it so the boss can fire every 3 ish seconds
        _bossHealth = 50; // Can set this in the inspector.
        _anim = GetComponent<Animator>(); // gets the animator for later.
        _death = 0; // resets the death cycle

        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>(); 
        if (_cameraShake == null) // Finds the main camera and checks if it is null
            Debug.LogError("Boss: Camera is null");
        _wait = new WaitForSeconds(.25f);
        if (_bossID == 2)
        {
            _wait = new WaitForSeconds(0.6f);
            StartCoroutine(LaserShower());
        }
    }
        
    

    // Update is called once per frame
    void Update()
    {
        if (_leftGun && _rightGun && _death == 0 && _bossID != 2)
        {//if left and right gun have been destroyed and we aren't dead
            if (_canFire <= Time.time) //if we can fire
            {
                StartCoroutine(BossLaser(_laserTransform)); //start the laser cannon up
            }
        }
        else if (_leftGun && _rightGun && _death == 0 && _bossID == 2)
        {
            if (_canFire <= Time.time) //if we can fire
            {
                StartCoroutine(BossLaser(_laserTransform)); 
                StartCoroutine(BossLaser(_leftGunTrans)); 
                StartCoroutine(BossLaser(_rightGunTrans)); 
                StartCoroutine(LaserShower());
            }
        }
        

        
        if (_bossHealth <= 0 && _death == 1) //if boss has no health and death phase 1 
        {            
            _death = 2; // sets death to 2 so can only get called once.
            StartCoroutine(BossDeath()); // start the explosions.
        }
    }

    public void LeftGun()
    {
        _leftGun = true;
        if (_rightGun == true)
        {
            transform.tag = "Enemy";
        }
    }

    public void RightGun()
    {
        _rightGun = true;
        if (_leftGun == true)
        {
            transform.tag = "Enemy";
        }
    }

    IEnumerator BossLaser(Transform Laser)
    {
        _canFire = Time.time + _fireRate; //sets the can fire  to time + firerate to give our cooldown for shooting
        //TODO
        GameObject newCharge =  Instantiate(_charge, Laser.position, Quaternion.identity); //spawn the charge for the laser
        newCharge.transform.parent = _laserTransform;// parent it to the lasertransform, this is so the charge will follow the boss as it moves

        yield return new WaitForSeconds(0.35f);//waits a bit for the charge to well, charge?
//TODO
        GameObject newLaser = Instantiate(_laser, Laser.position + new Vector3(0,-2.3f,0), Quaternion.identity);//spawn the laser
        newLaser.transform.parent = _laserTransform; //makes the laser move with boss.
        yield return new WaitForSeconds(1f); // waiting is key.
        // yield break;       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_rightGun && _leftGun && _death == 0 ) 
        {//if the right and left gun is broken and in death phase 0
            if (other.name == "Missile") //if hit by a missile
            {
                _bossHealth -= 20; //bonus damage! 
                Destroy(other.gameObject); // destroy the missile
            }
            else if (other.CompareTag("Laser")) //if hit by a laser
            {
                _bossHealth -= 10;// normal damage
                Destroy(other.gameObject);// destory the laser
            }
            if (_bossHealth <= 0) // if boss runs out of health
            {
                _death = 1; // move death to phase 1
            }          
        }
    }



    IEnumerator BossDeath()
    {
        _anim.enabled = false; //stops the boss from moving
        _explosion1.SetActive(true); //first explosion
        shakeSignal.SetValue(1);//shakes a little
        yield return new WaitForSeconds(1f);//waits a bit for the next boom
        shakeSignal.SetValue(2);// more shaky
        _explosion2.SetActive(true); //second explosion 
        yield return new WaitForSeconds(1f);//waits a bit for the next boom
        shakeSignal.SetValue(3);// even more shaky
        _explosion3.SetActive(true); //thrid explosion
        yield return new WaitForSeconds(1f);//waits a bit for the next boom
        shakeSignal.SetValue(4);// more shaky to the extreme!!!!
        //TODO
        Instantiate(_explosion4, transform.position, Quaternion.identity);// this spawns the last big explosion
        yield return new WaitForSeconds(0.3f);// gives it a sec before...
        Destroy(this.gameObject); //destroys the boss body
    }

    public void LaserRush()
    {
        StartCoroutine(LaserShower());
    }

    IEnumerator LaserShower()
    {
        //TODO
        Instantiate(_starfall, transform.position, Quaternion.identity);
        yield return _wait;
        Instantiate(_starfall, transform.position, Quaternion.identity);
        yield return _wait;
        Instantiate(_starfall, transform.position, Quaternion.identity);
        yield return _wait;
        Instantiate(_starfall, transform.position, Quaternion.identity);
        yield return _wait;
        Instantiate(_starfall, transform.position, Quaternion.identity);
        yield return _wait;
        Instantiate(_starfall, transform.position, Quaternion.identity);
        yield return _wait;
        Instantiate(_starfall, transform.position, Quaternion.identity);
        yield return _wait;
        Instantiate(_starfall, transform.position, Quaternion.identity);
    }

}
