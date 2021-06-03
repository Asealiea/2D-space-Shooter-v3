using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2.5f;
    [SerializeField] private float _destroyTimer = 2.7f;

    void Update()
    {
        transform.Translate(Vector3.down* _moveSpeed * Time.deltaTime);
        Destroy(this.gameObject, _destroyTimer);
    }
}
