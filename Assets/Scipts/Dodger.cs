using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodger : MonoBehaviour
{

    private bool left = false;
    private int _count;
    private bool _moving;
    [SerializeField] GameObject _core;

    // Update is called once per frame
    private void Update()
    {
        if (_core is null)
        {
            //Destroy(this.gameObject);
            ObjectPool.BackToPool(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Laser")) return;
        
        StartCoroutine(left ? MoveLeft() : MoveRight());
    }
       
    private IEnumerator MoveLeft()
    {
        if (_moving) yield break;
        _moving = true;
        transform.position = new Vector3(transform.position.x - 2, 5.5f, 0);
        _count++;
        yield return new WaitForSeconds(0.5f);
        if (_count >= 2)
        {
            left = false;
            _count = 0;
        }
        _moving = false;
    }

    private IEnumerator MoveRight()
    {
        if (_moving) yield break;
        _moving = true;
        transform.position = new Vector3(transform.position.x + 2, 5.5f, 0);
        _count++;
        yield return new WaitForSeconds(0.5f);
        if (_count >= 2)
        {         
            left = true;
            _count = 0;
        }
        _moving = false;
    }
}
