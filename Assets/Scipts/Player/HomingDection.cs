using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingDection : MonoBehaviour
{
    private Vector3 _ramDirection;
    [SerializeField] private bool _mine = false;
    [SerializeField] private bool _ram = false;
    private bool _enemyFound = false;
 [SerializeField]   private bool _playerFound = false;
    private Transform _enemy;
    private Transform _player;
#if _mine == false
    [SerializeField] private GameObject _thruster;
#endif
    [SerializeField] private GameObject _missile;
    [SerializeField] private GameObject _explosion;



    // Update is called once per frame
    void Update()
    {
        if (_mine || _ram)
        {            
            if (!_playerFound)
            {               
                if (!_mine)
                {
                    transform.Translate(Vector3.down * 5 * Time.deltaTime);
                }
                else
                {                    
                    transform.Translate(Vector3.down * 0.001f * Time.deltaTime);
                }
            }
            else
            {
                Debug.Log("player found calling movement");
                if (!_mine)
                {
                    Debug.Log("false mine called");
                    HighThrusters(true);
                    RamPlayer();
                }
                else
                {
                    Debug.Log("mine called");
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
        if (_missile == null)
            Destroy(this.gameObject);
        
    }
        
    private void FollowPlayer(Transform Playertransform)
    {
        Debug.Log("Follow player called");
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

    private void RamPlayer()
    {
        transform.Translate(_ramDirection * 5 * Time.deltaTime);
    }

    private void FollowEnemy(Transform Enemytransform)
    {
        if (_enemy != null)
        {            
            Vector3 direction = Enemytransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * 20);
      
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
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            Debug.Log("found player");
                _playerFound = true;
                _player = other.transform;
                //_ramDirection = _player.position - transform.position;
            
        }
        if (other.CompareTag("Laser"))
        {
            Destroy(this.gameObject);
        }
    }


    private void LowThrusters(bool missile)
    {
        if (missile)
        {
            _thruster.transform.localScale = new Vector3(0.15f, 0.25f, 0);
            _thruster.transform.position = transform.position + new Vector3(0f, 0.6f, 0f);
        }
        else
        {

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
        {
            _thruster.transform.localScale = new Vector3(0.2f, 0.56f, 0);
            _thruster.transform.position = transform.position + new Vector3(0f, 0.25f, 0f);
        }
    }

    IEnumerator MineDestory()
    {
        yield return new WaitForSeconds(5f);
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
          
         

}
