using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodger : MonoBehaviour
{

    private bool left = false;
    private int _count;
    private bool _moving;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.CompareTag("Laser"))
        {

            //if too close to right side move left
            //if too close to left side move right
            //if not  close to the left or right side, move in random left or right direction
            
            if (left)
            {
           
                
                StartCoroutine(MoveLeft());
              
            }
            else 
            {
                
                StartCoroutine(MoveRight());
      
            } 

       
        }
    }
    IEnumerator MoveLeft()
    {
        if (!_moving)
        {
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
            yield break;

        }
    }

    IEnumerator MoveRight()
    {
        if (!_moving)
        {
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
            yield break;
        }
    }
}
