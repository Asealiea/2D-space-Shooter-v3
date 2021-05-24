using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private WaitForSeconds _spawnDelay /*, _spawnDelaybg*/;
    [SerializeField] private float delaytime = 1f;
   // [SerializeField] private float delaytimebg = 1f;

    [SerializeField] private GameObject _enemyPreFab;
    private bool _spawn = true;
    [SerializeField] private GameObject _enemyContainer;
   // [SerializeField] private GameObject[] _backgroundScene;



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
