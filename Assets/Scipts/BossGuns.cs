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



    // Start is called before the first frame update
    void Start()
    {
        _gunExplode.SetActive(false);

        _gunHealth = 50;
        //find the player
        _player = GameObject.Find("Player").GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_gunHealth <= 0)
        {
            if (_gunSide == 1)
            {
                _boss.LeftGun();
            }
            else
            {
                _boss.RightGun();
            }
            _gunExplode.SetActive(true);
            _gun.SetActive(false);
        }
        if (_player == null)
        {
            Debug.LogError("BossGun: Player is null, disabling weapons");
            _gun.SetActive(false);
        }
        if (_canFire <=  Time.time)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        _fireRate = Random.Range(2.5f, 4f);
        _canFire = Time.time + _fireRate;
        //find where the player is and shoot towards the player.
        if (_player != null)
        {
            Vector3 direction = _player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 270;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);           
            Instantiate(_laser, transform.position, q);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
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
