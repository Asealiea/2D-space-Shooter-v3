using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{

    private float searchCooldown = 1;
    private Enemy _enemy;
    [SerializeField]   private bool _enemyFound = false;
    [SerializeField]  private CircleCollider2D _detection;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_enemyFound)
        {
                transform.Translate(Vector3.up * 4 * Time.deltaTime); 
    
            if (!EnemyIsAlive())
            {
                return;
            }
        }
        else
        {
            if (_enemy != null )
            {
                
                Vector3 direction = _enemy.transform.position - transform.position;

                Vector3 vectorToTarget = _enemy.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * 20);


                direction.Normalize();
                transform.Translate(direction * 10 * Time.deltaTime);

            }
            
           
        }
        if (transform.position.y > 8f)
        {
            Destroy(this.gameObject);
        }
        /*
        var damping:int= 2;
        var target:Transform;

        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        */

    }

    bool EnemyIsAlive()
    {
        searchCooldown -= Time.deltaTime;
        if (searchCooldown <= 0f)
        {
            searchCooldown = 0.5f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                
                return false;
            }
        }
            _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        _enemyFound = true;
        return true;
    }




    /*

    Homing projectile.
-Make a missile image,
-have it set up for another key, maybe E
Can have up to 3 missiles.
When they shoot out they look for a target, if none then just fly up. (could use the search funtion of the wave system)
when no target slowly fly up, maybe use smaller thruster
when spots a enemy, bigger thruster and move faster and change direction towards enemy.
*/
}
