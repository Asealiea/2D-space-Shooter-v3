using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToPool : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TestCube") || collision.gameObject.CompareTag("TestSphere"))
        {
            DangryGames.ObjectPooling.ObjectPool.SharedInstance.BackToPool(collision.gameObject);
        }
    }
}
