using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGuns : MonoBehaviour
{
    [SerializeField] private GameObject _gun;  //holds the gun it's attached to, can disable it when it gets destroyed
    [SerializeField] private GameObject _gunExplode; //holds the guns damaged effect, can enable it when gun gets destroyed
    [SerializeField] private int _gunHealth; //Stores how much Health the guns have before they go boom
    [SerializeField] private Player _player; //used to track the players position to be able to shoot towards them.
    [SerializeField] private float _canFire = 6; //added in a delay so the Boss can get onto the screen before it attacks
    private float _fireRate; // how fast the boss can attack 
    [SerializeField] private GameObject _laser; //the laser for the gun to fire.
    [SerializeField] private Boss _boss; //Reference to the boss for use later
    [SerializeField] private int _gunSide; // 1 = left 2= right
    [SerializeField] private int _bossNumber;
    private float _rotation;
    



    // Start is called before the first frame update
    void Start()
    {
        _gunExplode.SetActive(false); //used this to make sure it wasn't active when the boss came out
        _gunHealth = 50; //can be set in the inspector
        //find the player
        _player = GameObject.Find("Player").GetComponent<Player>(); // finds the player to use later
        if (_gunSide == 1)
        {
            _rotation = 15f;
        }
        else
        {
            _rotation = -15f;
        }
        if (_bossNumber == 2)
        {
            StartCoroutine(RotatorShoot());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gunHealth <= 0) // if the gun has been destroyed....
        {
            if (_gunSide == 1) //set in the inspector for each side
            {
                _boss.LeftGun(); //turns off the guns on the boss
            }
            else // when both guns are off you can start attacking the main boss
            {
                _boss.RightGun();//turns off the guns on the boss
            }
            _gunExplode.SetActive(true); //turns on the damage effects
            _gun.SetActive(false); // turns off the gun so it can't keep shooting.
        }

        /* if (_player == null) //null check, also 
         {
             Debug.LogError("BossGun: Player is null, disabling weapons");
             _gun.SetActive(false);
         } */
        if (_bossNumber == 1)
        {
            if (_canFire <=  Time.time) //so we don't see a death stream of bullets.            
                Shoot(); // just a normal shooting method, nothing to see here.            
        }

    }

    private void Shoot()
    {
        _fireRate = Random.Range(2.5f, 4f); //sets a random firerate.
        _canFire = Time.time + _fireRate; //sets the time to be shoot again.

        //find where the player is and shoot towards the player.
        if (_player != null)
        {
            Vector3 direction = _player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 270;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);     
            //TODO
            Instantiate(_laser, transform.position, q);
        }
    }


    IEnumerator RotatorShoot()
    {
        while (true)
        {
            //TODO
            Instantiate(_laser, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.25f);
            transform.Rotate(0, 0, _rotation);
            //TODO
            Instantiate(_laser, transform.position, transform.rotation) ;
            yield return new WaitForSeconds(0.25f);
            transform.Rotate(0, 0, _rotation);
        }        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Missile")
        {
            if (_gunHealth >= 10)
            {
                _gunHealth -= 20;
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Laser"))
        {
            if (_gunHealth >= 10)
            {
                _gunHealth -= 10;
            }
            Destroy(other.gameObject);
        }

    }
}
