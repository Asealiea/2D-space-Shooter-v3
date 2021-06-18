using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingDection : MonoBehaviour
{
    private bool _found = false;
    private Transform _enemy;
    [SerializeField] private GameObject _thruster;
    [SerializeField] private GameObject _missile;



    // Update is called once per frame
    void Update()
    {
        if (!_found)
        {
            transform.Translate(Vector3.up * 5 * Time.deltaTime);
            LowThrusters();
        }
        else
        {
            FollowEnemy(_enemy);
            HighThrusters();
        }

        if (transform.position.y >= 10 || transform.position.y <= -10f)
            Destroy(this.gameObject);
        if (transform.position.x >= 10 || transform.position.x <= -10f)
            Destroy(this.gameObject);
        if (_missile == null)
            Destroy(this.gameObject);
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
            _found = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!_found)
            {
                _found = true;

                _enemy = other.transform;
            }
        }
    }

    private void LowThrusters()
    {
        _thruster.transform.localScale = new Vector3(0.15f, 0.25f, 0);
        _thruster.transform.position = transform.position + new Vector3(0f, 0.6f, 0f);
    }
    private void HighThrusters()
    {
        _thruster.transform.localScale = new Vector3(0.25f, .5f, 0);
        _thruster.transform.position = transform.position + new Vector3(0f, 0.25f, 0f);
    }
          
         

}
