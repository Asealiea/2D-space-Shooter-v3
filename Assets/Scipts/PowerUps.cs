using System;
using System.Collections;
using System.Collections.Generic;
using RSG.Trellis.Signals;
using UnityEngine;

public class PowerUps : MonoBehaviour
{

    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _spinningSpeed = 0;
    [SerializeField] private int _powerUpID; //0 = triple shot, 1 = speed, 2 = Shield, 3 = Ammo Refill, 4 = Extra life, 5 = secondary fire
    [SerializeField] private AudioClip _powerUpClip;
    [SerializeField] private Transform _mag;

    [SerializeField] private BoolSignal tripleShotSignal;
    [SerializeField] private BoolSignal speedSignal;
    [SerializeField] private BoolSignal shieldsSignal;
    [SerializeField] private BoolSignal ammoSignal;
    [SerializeField] private BoolSignal lifeSignal;
    [SerializeField] private BoolSignal missileSignal;
    [SerializeField] private BoolSignal negativeSignal;
    [SerializeField] private BoolSignal superSignal;

    private float _movement;
    private float _magMove;

    void Update()
    {
        if (_mag)
        {
            transform.rotation = Quaternion.identity;
            _spinningSpeed = 0;
            Vector3 direction = _mag.position - transform.position;
            direction.Normalize();
            _magMove = (_speed * 2) * Time.deltaTime;
            transform.Translate(direction * _magMove);

            return;
        }

        transform.Rotate(0, _spinningSpeed, 0);
        _movement = _speed * Time.deltaTime;
        transform.Translate(Vector3.down * _movement);
        if (transform.position.y <= -4.5f)
            ObjectPool.BackToPool(this.gameObject);
            //Destroy(this.gameObject);
        


        
    }

    public void Magnet(Transform Player)
    {
        _mag = Player;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyLaser"))
        {
            ObjectPool.BackToPool(other.gameObject);
            if (/*transform.parent != null &&*/ transform.CompareTag("Missile"))
            {
                //Destroy(transform.parent.gameObject); // need for missiles
                ObjectPool.BackToPool(this.transform.parent.gameObject);
            }
            ObjectPool.BackToPool(this.gameObject);
        }

        if (!other.CompareTag("Player")) return;
        
        AudioSource.PlayClipAtPoint(_powerUpClip, transform.position);
        
        switch (_powerUpID)
        {
            case 0: //Triple Shot
                tripleShotSignal.SetValue(true);
                break;
            case 1://Speed
                speedSignal.SetValue(true);
                break;
            case 2: //Shields
                shieldsSignal.SetValue(true);
                break;
            case 3://Ammo Refill
                ammoSignal.SetValue(true);
                break;
            case 4://extra Life.
                lifeSignal.SetValue(true);
                break;
            case 5://extra Missile
                missileSignal.SetValue(true);
                break;
            case 6: // negative power up aka powerdown
                negativeSignal.SetValue(true);
                break;
            case 7: //keep this one last.
                superSignal.SetValue(true);
                break;
            default:
                break;
        }
        
        if (transform.parent != null)
        {
            //Destroy(transform.parent.gameObject, 1);
        }

        _mag = null;
        ObjectPool.BackToPool(this.gameObject);
    }
}
