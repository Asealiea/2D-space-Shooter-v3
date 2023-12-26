using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private float _spinningSpeed = 10;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Background _bg;
    private float temp;

    // Start is called before the first frame update
    private void Start()
    {

        //_gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
        {
            Debug.Log("Asteroid: GameManager is null");
        }
            
        //_bg = GameObject.Find("Bottom").GetComponent<Background>();
        _bg = FindObjectOfType<Background>();
        if (_bg == null)
        {
            Debug.Log("Asteroid: Background is null");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        temp = _spinningSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward * temp);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Laser")) return;
        
        ObjectPool.SpawnObject(transform.position,Quaternion.identity, "PlayerDeath");
        ObjectPool.BackToPool(other.gameObject);
        _gameManager.StartGame();
        _bg.StartMoving();
        Destroy(this.gameObject,0.25f);
    }
}
