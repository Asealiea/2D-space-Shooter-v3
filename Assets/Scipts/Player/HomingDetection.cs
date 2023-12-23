using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingDetection : MonoBehaviour
{
    private Quaternion _defaultQuaternion;
    private int _count;
    private Vector3 _ramDirection;
    [SerializeField] private bool _mine = false;
    [SerializeField] private bool _ram = false;
    [SerializeField] private bool _missiles;
    [SerializeField]   private bool _playerFound = false;
    private bool _enemyFound = false;
    private Transform _enemy;
    private Transform _player;
#if _mine == false
    [SerializeField] private GameObject _thruster;
#endif
    [SerializeField] private GameObject _coreComponent;
    [SerializeField] private GameObject _explosion;

    private void Start()
    {
        if (_ram)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 8f, 0);
            _defaultQuaternion = transform.rotation;
        }
    }


    // Update is called once per frame
    void Update()
    {
     

        if (_mine || _ram)
        {
            if (!_playerFound)
            {
                if (!_mine)
                    transform.Translate(Vector3.down * 3 * Time.deltaTime);
                else
                    transform.Translate(Vector3.down * 0.01f * Time.deltaTime);
            }
            else
            {
                if (!_mine)
                {
                    HighThrusters(false);
                    RamPlayer();
                }
                else
                {
                    FollowPlayer(_player);
                    StartCoroutine(MineDestory());
                }
            }
        }

        if (!_mine && !_ram)
        {
            if (!_enemyFound)
            {
                transform.Translate(Vector3.up * 5 * Time.deltaTime);
                LowThrusters(true);
            }
            else
            {
                FollowEnemy(_enemy);
                HighThrusters(true);
            }
        }
        
        if (transform.position.y >= 10 || transform.position.y <= -10f)
            Destroy(this.gameObject);
        if (transform.position.x >= 10 || transform.position.x <= -10f)
            Destroy(this.gameObject);
        if (_coreComponent == null)
            Destroy(this.gameObject);
        
    }
        
    private void FollowPlayer(Transform Playertransform) //mine 
    {    
        if (_player != null)
        {
            Vector3 direction = Playertransform.position - transform.position;

            direction.Normalize();
            transform.Translate(direction * 4 * Time.deltaTime);   
        }
        else
        {
            _playerFound = false;
        }
    }

    private void RamPlayer() //ramer
    {
    
        // get the players pos
        if (_count == 0)
        {
            _ramDirection = _player.position - transform.position;
            _count = 1;
        }

        //face the player
        if (transform.position.y >= _player.position.y && _count == 1)
        {
            float angle = Mathf.Atan2(_ramDirection.y, _ramDirection.x) * Mathf.Rad2Deg - 270;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * 2);
            if (_playerFound)
            {
                _ramDirection.Normalize();
                transform.Translate(_ramDirection * 5 * Time.deltaTime);
            }
        }
        else
        {
            _playerFound = false;
        }


        //ram the player.
        if (!_playerFound)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _defaultQuaternion , Time.deltaTime * 20);

            _count = 0;
        }



    }

    private void FollowEnemy(Transform Enemytransform) //missiles
    {
        if (_enemy != null)
        {            
            Vector3 direction = Enemytransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * 5);
      
            direction.Normalize();
            transform.Translate(direction * 20 * Time.deltaTime);
        }
        else
        {
            _enemyFound = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy"))
        {
            if (!_enemyFound)
            {
                _enemyFound = true;
                _enemy = other.transform;
            }
        }

        if (other.CompareTag("Player"))
        {
            _playerFound = true;
            _player = other.transform;            
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerFound = false;
        }
    }


    private void LowThrusters(bool missile)
    {
        if (missile)
        {
            _thruster.transform.localScale = new Vector3(0.15f, 0.25f, 0);
            _thruster.transform.position = transform.position + new Vector3(0f, 0.6f, 0f);
        }
    }

    private void HighThrusters(bool Missile)
    {
        if (Missile)
        {
            _thruster.transform.localScale = new Vector3(0.25f, .5f, 0);
            _thruster.transform.position = transform.position + new Vector3(0f, 0.25f, 0f);
        }
        else        
           return;        
    }

    IEnumerator MineDestory()
    {
        yield return new WaitForSeconds(5f);
        //pool these both.
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }



}
