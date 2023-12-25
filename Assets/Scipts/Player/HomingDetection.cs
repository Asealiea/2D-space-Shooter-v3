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
    [SerializeField] private GameObject _thruster;
    [SerializeField] private GameObject _coreComponent;
    [SerializeField] private GameObject _explosion;
    private readonly Vector3 _lowScale = new Vector3(0.15f, 0.25f, 0);
    private readonly Vector3 _lowPos   = new Vector3(0f, 0.6f, 0f);
    private readonly Vector3 _highScale = new Vector3(0.25f, .5f, 0);
    private readonly Vector3 _highPos   = new Vector3(0f, 0.25f, 0f);
    private float temp;
    private GameObject obj;


    private void Start()
    {
        if (_ram)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 8f, 0);
            _defaultQuaternion = transform.rotation;
        }
    }

    private void RamAndMine()
    {
        if (!_playerFound)
        {
            if (!_mine)
            {
                temp = 3 * Time.deltaTime;
                transform.Translate(Vector3.down * temp);
            }
            else
            {
                temp = 0.01f * Time.deltaTime;
                transform.Translate(Vector3.down * temp);
            }
        }
        else
        {
            if (!_mine)
            {
                //LowThrusters(true); before it was just returning
                RamPlayer();
            }
            else
            {
                FollowPlayer(_player);
                StartCoroutine(MineDestroy());
            }
        }
    }

    // Update is called once per frame

    private void Update()
    {
        if (_mine || _ram)
        {
            RamAndMine();
        }

        if (!_mine && !_ram)
        {
            if (!_enemyFound)
            {
                temp = 5 * Time.deltaTime;
                transform.Translate(Vector3.up * temp);
                LowThrusters(true);
            }
            else
            {
                FollowEnemy(_enemy);
                LowThrusters(false);
            }
        }
        
        if (transform.position.y >= 10 || transform.position.y <= -10f)
            ObjectPool.BackToPool(this.gameObject);
        if (transform.position.x >= 10 || transform.position.x <= -10f)
            ObjectPool.BackToPool(this.gameObject);
        if (_coreComponent == null)
            ObjectPool.BackToPool(this.gameObject);
        
    }


    private void FollowPlayer(Transform playerTransform) //mine 
    {    
        if (_player != null)
        {
            Vector3 direction = playerTransform.position - transform.position;

            direction.Normalize();
            temp = 4 * Time.deltaTime;
            transform.Translate(direction * temp);   
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
                temp = 5 * Time.deltaTime;
                transform.Translate(_ramDirection * temp);
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

    private void FollowEnemy(Transform enemyTransform) //missiles
    {
        if (_enemy != null)
        {            
            Vector3 direction = enemyTransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * 5);
      
            direction.Normalize();
            temp = 20 * Time.deltaTime;
            transform.Translate(direction * temp);
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

        if (!other.CompareTag("Player")) return;
        _playerFound = true;
        _player = other.transform;
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
            _thruster.transform.localScale = _lowScale;
            _thruster.transform.position = transform.position + _lowPos;
        }
        else
        {
            _thruster.transform.localScale = _highScale;
            _thruster.transform.position = transform.position + _highPos;
        }
    }

    private IEnumerator MineDestroy()
    {
        yield return new WaitForSeconds(5f);
        obj = ObjectPool.SharedInstance.GetPooledObject("EnemyExplosion");
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

        ObjectPool.BackToPool(this.gameObject);
    }



}
