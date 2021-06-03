using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private WaitForSeconds _spawnDelay;
    [SerializeField] private float _delaytime = 1f;

    private bool _spawn = true;
    private float _randomWait;
    private int _powerID;


    [Header("Game Objects")]
    [SerializeField] private GameObject _enemyPreFab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerUpContainer;



    /*
    [Header ("Background Objects")]
    [SerializeField] private float delaytimebg = 4f;
    private WaitForSeconds  _spawnDelaybg;
    [SerializeField] private float delaytimebgSmall = 2f;
    private WaitForSeconds  _spawnDelaybgSmall;
    [SerializeField] private GameObject[] _backgroundScene;
    [SerializeField] private GameObject[] _smallBackGroundScene;
    */


    // Start is called before the first frame update
    void Start()
    {
    
        _spawnDelay = new WaitForSeconds(_delaytime);

        //  _spawnDelaybg = new WaitForSeconds(delaytimebg);
        //  _spawnDelaybgSmall = new WaitForSeconds(delaytimebgSmall);

        NullCheck();


        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerUpRoutine());


        // StartCoroutine(BackGroundObjects());
        //StartCoroutine(SmallBackgroundObjects());
    }
/*
    IEnumerator BackGroundObjects()
    {
        while (_spawn == true)
        {
            yield return _spawnDelaybg;
            int randomBg = Random.Range(0, _backgroundScene.Length);
            Instantiate(_backgroundScene[randomBg], transform.position, Quaternion.identity);
        }
    }
     
    IEnumerator SmallBackgroundObjects()
    {
        while (_spawn == true)
        {
            yield return _spawnDelaybg;
            int randomBg = Random.Range(0, _smallBackGroundScene.Length);
            Instantiate(_smallBackGroundScene[randomBg], transform.position, Quaternion.identity);
        }
    } */
    
    IEnumerator SpawnRoutine()
    {
        while (_spawn == true)
        {
            yield return _spawnDelay;
            GameObject newEnemy = Instantiate(_enemyPreFab, transform.position, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_spawn == true)
        {
            Vector3 randomX = new Vector3(Random.Range(-7f, 7f), 8, 0);
            _powerID = Random.Range(0, _powerUpContainer.Length);
            Instantiate(_powerUpContainer[_powerID], randomX, Quaternion.identity);
            _randomWait = (Random.Range(5f, 10f));
            yield return new WaitForSeconds(_randomWait);
        }
    }

    public void StopSpawning()
    {
        _spawn = false;
    }

    public void NullCheck()
    {
        if (_enemyPreFab == null)
            Debug.LogError("SpawnManager:Enemy PreFab is null");
        if (_enemyContainer == null)
            Debug.LogError("SpawnManager:Enemy Container is null");
        if (_powerUpContainer == null)
            Debug.LogError("SpawnManager:PowerUp Container is null");

    }
}
