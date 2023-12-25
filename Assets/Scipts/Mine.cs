using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    private GameObject obj;

    public void DestroyMine()
    {
        obj = ObjectPool.SharedInstance.GetPooledObject("EnemyExplosion");
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

        ObjectPool.BackToPool(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectPool.BackToPool(this.gameObject);
        }

        if (!other.CompareTag("Laser")) return;
        obj = ObjectPool.SharedInstance.GetPooledObject("EnemyExplosion");
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

        ObjectPool.BackToPool(this.gameObject);
    }
}
