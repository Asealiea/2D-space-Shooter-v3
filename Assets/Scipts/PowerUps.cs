using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{

    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _spinningSpeed = 0;
    [SerializeField] private int _powerUpID; //0 = triple shot, 1 = speed, 2 = Shield, 3 = Ammo Refill, 4 = Extra life, 5 = secondary fire
    [SerializeField] private AudioClip _powerUpClip;
    [SerializeField] private Transform _mag;


     
    void Update()
    {
        if (_mag)
        {
            transform.rotation = Quaternion.identity;
            _spinningSpeed = 0;
            Vector3 direction = _mag.position - transform.position;
            direction.Normalize();
            transform.Translate(direction * (_speed * 2) * Time.deltaTime);

            return;
        }

        transform.Rotate(0, _spinningSpeed, 0);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -4.5f)
            Destroy(this.gameObject);
        


        
    }

    public void Magnet(Transform Player)
    {
        _mag = Player;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ELaser"))
        {
            Destroy(other.gameObject);
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject); // need for missiles
            }
            Destroy(this.gameObject);        
        }

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_powerUpClip, transform.position);
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
                    case 3://Ammo Refill
                        player.ExtraAmmo();
                        break;
                    case 4://extra Life.
                        player.ExtraLife();
                        break;
                    case 5://extra Missile
                        player.MissilePayload();
                        break;
                    case 6: // negative power up aka powerdown
                        player.NegativePowerUp();
                        break;
                    case 7: //keep this one last.
                        player.SecondaryFire(); // starburst.
                        break;
                    default:
                        break;
                }
            }
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject, 1);
            }
            Destroy(this.gameObject);
        }
    }
}
