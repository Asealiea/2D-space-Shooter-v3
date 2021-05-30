using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{

    [SerializeField] private float _speed = 3f;

    [SerializeField] private int _powerUpID; //0 = triple shot, 1 = speed, 2 = Shield;
  
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerUpID)
                {
                    case 0: //Triple Shot
                        player.TripleShotActive();
                        break;
                    case 1://Speed
                        player.SpeedActive();
                        break;
                    case 2: //Shields
                        player.ShieldsActive();
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
