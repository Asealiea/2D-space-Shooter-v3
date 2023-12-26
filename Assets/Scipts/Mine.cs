using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;

    public void DestroyMine()
    {
        ObjectPool.SpawnObject(transform.position ,Quaternion.identity, "EnemyExplosion");
        ObjectPool.BackToPool(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectPool.BackToPool(this.gameObject);
        }

        if (!other.CompareTag("Laser")) return;
        ObjectPool.SpawnObject(transform.position ,Quaternion.identity, "EnemyExplosion");
        ObjectPool.BackToPool(this.gameObject);
    }
}
