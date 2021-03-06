using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool _dodger;
    [SerializeField] private float _speed = 4;
    private Player _player;
    [SerializeField]  private float _canFire = 2f;
    private float _fireRate;

    [SerializeField] private int _enemyID = 0;
    [SerializeField] private int _leftOrRight;
    [SerializeField] private Animator _anim;

    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject _enemyLaser;

    [SerializeField] private GameObject _ammoRefill;


    [SerializeField] private bool _hasShields = false;
    [SerializeField] private GameObject _shields;

    private Vector3 Direction;




    
    void Start()
    {
 
        if (_enemyID < 3)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 8f, 0);           
        }

        if (_enemyID == 2)
            _leftOrRight = Random.Range(1, 3);

 
        _player = GameObject.Find("Player").GetComponent<Player>();
        
        _anim = GetComponent<Animator>();

        
        if (_player == null)
        {
            Debug.LogError("Enemy: Player is null");
            Destroy(this.gameObject);
        }
        if (_shields == null)
            Debug.LogError("Enemy: Shields is null");


        if (_enemyID == 4)
        {
            transform.position = new Vector3(Random.Range(-7f, 7f), 5.5f, 0f);
        }

        if (_anim == null)
            Debug.Log("Enemy: Animator is null");
       
    }
    
    void Update()
    {
        switch (_enemyID)
        {
            case 0: //normal movement
                _anim.enabled = false; 
                EnemyMovement();
                break;
            case 1: //arching side to side
                _anim.SetTrigger("SidewaysMovement");
                _fireRate = 2f;
                if (_player == null)
                    Destroy(this.gameObject);
                break;
            case 2://moves in a sinwave kind of movement across screen         
                _anim.SetTrigger("SinWaveMovement");
                if (_leftOrRight == 1)
                    _anim.SetTrigger("SinWaveLeft");
                else
                    _anim.SetTrigger("SinWaveRight");
                if (_player == null)
                    Destroy(this.gameObject);               
                break;
            case 3: // ramer
                //ram player when it gets close enough.
                _anim.enabled = false;
                break;
            case 4: //dodger
                if (_player == null)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    FacethePlayer(_player.transform);
                    if (Time.time >= _canFire)
                    {
                        EnemyFacetoShoot();
                    }

                }
                break;


            default:
                _anim.enabled = false;
               // EnemyMovement();
                break;
        }

        if (_enemyID < 3) // ramer can't fire
        {
            if (Time.time >= _canFire)    //_canFire == true
                EnemyShoot();            
        }
        if (_player == null)
        {
            Destroy(this.gameObject);
        }
    }


    private void EnemyShoot()
    {
        _fireRate = Random.Range(2f, 4f);
        _canFire = Time.time + _fireRate;
        Instantiate(_enemyLaser, transform.position + new Vector3(0f, -1.3f, 0f), Quaternion.identity);
    }

    private void EnemyFacetoShoot()
    {
        _fireRate = Random.Range(2f, 3f);
        _canFire = Time.time + _fireRate;

        Direction = _player.transform.position - transform.position;
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg - 270;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
     //   Instantiate(_enemyLaser, transform.position + new Vector3(0f, -1.3f, 0f), q);
        Instantiate(_enemyLaser, transform.position, q);
    }


    private void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -5.5f && _player != null)
        {
            float _randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(_randomX, 8f, 0);
        }
        else if (transform.position.y <= -5.5f && _player == null)
        {
            Destroy(this.gameObject);
        }
    }

    private void FacethePlayer(Transform Player)
    {
        Direction = Player.position - transform.position;
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg - 270;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * 20);
    }

     private void OnTriggerEnter2D(Collider2D other)
     {
         if (other.CompareTag("Player"))
         {
            if (!_hasShields)
            {
                // Player player = other.transform.GetComponent<Player>();
                 if (_player != null)
                 {
                     _player.Damage(true);
                 }
                int randomDrop = Random.Range(0, 11);
                if (randomDrop >= 7)
                {
                    Instantiate(_ammoRefill, transform.position, Quaternion.identity);
                }
                //_anim.SetTrigger("OnEnemyDeath");
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                //Destroy(this.gameObject, 2.7f);// add delay to the destory
            }
            else
            {
                _hasShields = false;
                _shields.SetActive(false);
                if (_player != null)
                {
                    _player.Damage(true);
                }
            }
         }

        if (other.CompareTag("Laser"))
         {
            if (!_hasShields) //if they don't have a shield
            {
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                Destroy(other.gameObject);
                //spawn an ammo refill on chance
                int randomDrop = Random.Range(0, 11);
                if (randomDrop >= 6)
                {
                    Instantiate(_ammoRefill, transform.position, Quaternion.identity);
                }
                Instantiate(_explosion, transform.position, Quaternion.identity); //instantiate the explosion
                Destroy(this.gameObject);// no delay needed for this.               
            }
            else //if they do have a shield.
            {
                _hasShields = false;
                _shields.SetActive(false);
                Destroy(other.gameObject);
            }
        }
     }

}
