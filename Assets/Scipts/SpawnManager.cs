using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private WaitForSeconds _spawnDelay;
    [SerializeField] private float delaytime = 1f;

    private bool _spawn = true;


    [Header("Game Objects")]
    [SerializeField] private GameObject _enemyPreFab;
    [SerializeField] private GameObject _enemyContainer;


    /*
    [Header ("Background Objects")]
    [SerializeField] private float delaytimebg = 1f;
    private WaitForSeconds  _spawnDelaybg;
    [SerializeField] private GameObject[] _backgroundScene;
    */


    // Start is called before the first frame update
    void Start()
    {
        
        _spawnDelay = new WaitForSeconds(delaytime);
      //  _spawnDelaybg = new WaitForSeconds(delaytimebg);
        if (_enemyPreFab == null)
        {
            Debug.LogError("SpawnManager:No Enemy Prefab");
        }
        StartCoroutine(SpawnRoutine());
       // StartCoroutine(BackGroundObjects());
    }
/*
    IEnumerator BackGroundObjects()
    {
        while (_spawn == true)
        {
            yield return _spawnDelaybg;
            int randomBg = Random.Range(0, _backgroundScene.Length + 1);
            Instantiate(_backgroundScene[randomBg], transform.position, Quaternion.identity);
           
        }
    }
    */
    IEnumerator SpawnRoutine()
    {
        while (_spawn == true)
        {
            yield return _spawnDelay;

            GameObject newEnemy = Instantiate(_enemyPreFab, transform.position, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }
        
    }

    public void StopSpawning()
    {
        _spawn = false;
    }

}
