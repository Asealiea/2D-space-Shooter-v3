using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private float _spinningSpeed = 10;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Background _bg;


    // Start is called before the first frame update
    void Start()
    {

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.Log("Asteroid: GameManager is null");


        _bg = GameObject.Find("Bottom").GetComponent<Background>();
        if (_bg == null)
            Debug.Log("Asteroid: Background is null");
        
        
        
        
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
            _bg.StartMoving();
            Destroy(this.gameObject,0.25f);
        }
    }
}
