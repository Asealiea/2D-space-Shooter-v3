using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float fireSpeed;
    private float _moveSpeed;


    private void OnEnable()
    {
        _moveSpeed = fireSpeed * Time.deltaTime;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * _moveSpeed);
        
        if (transform.position.y >= 8)
        {
            if (transform.CompareTag("TripleShotAttack"))
            {
                ObjectPool.BackToPool(transform.parent.gameObject);
            }
            ObjectPool.BackToPool(this.gameObject);
        }
        if (transform.position.y <= -5.5)
        {
            ObjectPool.BackToPool(this.gameObject);
        }
    }

}
