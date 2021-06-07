using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2.5f;
    [SerializeField] private float _destroyTimer = 2.7f;
    [SerializeField] private AudioClip _explosionClip;
    [SerializeField] private AudioSource _explsionSource;

    private void Start()
    {
        _explsionSource = GetComponent<AudioSource>();
        if (_explsionSource == null)
        {
            Debug.Log("EnemyDeath: Audio Source is null");
        }
        else
        {
            _explsionSource.clip = _explosionClip;
        }
        _explsionSource.Play();
    }

    void Update()
    {
        transform.Translate(Vector3.down* _moveSpeed * Time.deltaTime);
        Destroy(this.gameObject, _destroyTimer);
    }


}
