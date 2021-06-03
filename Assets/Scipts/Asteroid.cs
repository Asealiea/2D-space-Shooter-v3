using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private float _spinningSpeed = 10;
    [SerializeField] private GameManager _gameManager;


    // Start is called before the first frame update
    void Start()
    {
        if (_gameManager == null)
            Debug.Log("Asteroid: GameManager is null");
        
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _spinningSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _gameManager.StartGame();
            Destroy(this.gameObject,0.25f);
        }
    }
}
