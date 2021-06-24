using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    //[SerializeField] private GameObject _leftGun, _rightGun;
    [SerializeField] private bool _leftGun, _rightGun = false;
    [SerializeField] private GameObject _charge, _laser;
    private float _canFire;
    private float _fireRate;
    [SerializeField]    private int _bossHealth;
    private int _death;
    [SerializeField] private GameObject _explosion1, _explosion2, _explosion3, _explosion4;
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private Transform _laserTransform;
    [SerializeField] private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _canFire = 1;
        _fireRate = 3;
        _bossHealth = 50;
        _anim = GetComponent<Animator>();
        _death = 0;

    
    }

    // Update is called once per frame
    void Update()
    {
        if (_leftGun && _rightGun && _death == 0)
        {
            if (_canFire <= Time.time)
            {
                StartCoroutine(BossLaser());
            }
        }
        if (_bossHealth <= 0)
        {
            if (_death == 1)
            {
                _death = 2;
                StartCoroutine(BossDeath());
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(BossLaser());
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

    IEnumerator BossLaser()
    {
        _canFire = Time.time + _fireRate;
        GameObject newCharge =  Instantiate(_charge, _laserTransform.position, Quaternion.identity);
        newCharge.transform.parent = _laserTransform;

        yield return new WaitForSeconds(0.35f);

        GameObject newLaser = Instantiate(_laser, _laserTransform.position + new Vector3(0,-2.3f,0), Quaternion.identity);
        newLaser.transform.parent = _laserTransform;
        yield return new WaitForSeconds(1f);
        // yield break;


       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_rightGun && _leftGun && _death == 0 )
        {
            if (other.name == "Missile")
            {
                _bossHealth -= 20;
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("Laser"))
            {
                _bossHealth -= 10;
                Destroy(other.gameObject);
            }
            if (_bossHealth <= 0)
            {
                _death = 1;
            }
          
        }
    }

    IEnumerator BossDeath()
    {
        _anim.enabled = false;
        _explosion1.SetActive(true);
        _cameraShake.CameraShaker(1);
        yield return new WaitForSeconds(1f);
        _cameraShake.CameraShaker(2);
        _explosion2.SetActive(true);
        yield return new WaitForSeconds(1f);
        _cameraShake.CameraShaker(3);
        _explosion3.SetActive(true);
        yield return new WaitForSeconds(1f);
        _cameraShake.CameraShaker(4);
        Instantiate(_explosion4, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);

        yield break;
    }

}
