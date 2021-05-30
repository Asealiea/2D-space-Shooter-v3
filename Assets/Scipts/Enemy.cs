using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    [SerializeField] private float _speed = 4;

    private Player _player;
  
    
    void Start()
    {
        transform.position = new Vector3(Random.Range(-9f, 9f), 8f, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();

    }
    
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.5f)
        {
            float _randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(_randomX,8f, 0);
        }
    }

     private void OnTriggerEnter2D(Collider2D other)
     {
         if (other.CompareTag("Player"))
         {

            // Player player = other.transform.GetComponent<Player>();
             if (_player != null)
             {
                 _player.Damage();
             }
             Destroy(this.gameObject);
         }

         if (other.CompareTag("Laser"))
         {
            
            if (_player != null)
            {
                _player.AddScore(10);
            }
          
            Destroy(other.gameObject);
            Destroy(this.gameObject);
         }
     }


   

}
