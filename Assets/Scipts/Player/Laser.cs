using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float fireSpeed;
    private float _moveSpeed;


    private void Update()
    {
        transform.Translate(fireSpeed * Time.deltaTime * Vector3.up);

        if (transform.position.y >= 8)
        {
            if (transform.parent.CompareTag("TripleShotAttack"))
            {
                ObjectPool.BackToPool(transform.parent.gameObject);
                return;
            }
            ObjectPool.BackToPool(this.gameObject);
        }
        if (transform.position.y <= -5.5)
        {
            ObjectPool.BackToPool(this.gameObject);
        }
    }

}
