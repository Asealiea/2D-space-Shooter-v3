using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //[SerializeField] private Animator _anim;


    [SerializeField] private float _speed = 4;
    private Player _player;
    private float _canFire = -1f;
    private float _fireRate;

    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject _enemyLaser;
  
    
    void Start()
    {
        transform.position = new Vector3(Random.Range(-9f, 9f), 8f, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
       // _anim = GetComponent<Animator>();
        
        if (_player == null)
            Debug.Log("Enemy: Player is null");
        
        
        StartCoroutine(ShootEnemy());
        
       // if (_anim == null)
       //     Debug.Log("Enemy: Animator is null");
       
    }
    
    void Update()
    {
        EnemyMovement();

        if (Time.time > _canFire)    //_canFire == true
        {
            EnemyShoot();
        }
    }

    private void EnemyShoot()
    {
        _fireRate = Random.Range(3f, 7f);
        _canFire = Time.time + _fireRate;
        Instantiate(_enemyLaser, transform.position + new Vector3(0f, -1.3f, 0f), Quaternion.identity);

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

     private void OnTriggerEnter2D(Collider2D other)
     {
         if (other.CompareTag("Player"))
         {
            // Player player = other.transform.GetComponent<Player>();
             if (_player != null)
             {
                 _player.Damage(1);
             }
            //_anim.SetTrigger("OnEnemyDeath");
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        //Destroy(this.gameObject, 2.7f);// add delay to the destory
         }

        if (other.CompareTag("Laser"))
         {
            if (_player != null)
            {
                _player.AddScore(10);
            }
            Destroy(other.gameObject);
            //_anim.SetTrigger("OnEnemyDeath"); trigger not needed either.
            
            Instantiate(_explosion, transform.position, Quaternion.identity); //instantiate the explosion
            Destroy(this.gameObject);// no delay needed for this.
            //Destroy(this.gameObject, 2.7f);//add delay to destory
        }
     }
         
            

    IEnumerator ShootEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 4f));
            Instantiate(_enemyLaser, transform.position + new Vector3(0f,-1.3f,0f), Quaternion.identity);
        }

    }
 
           

   

}
