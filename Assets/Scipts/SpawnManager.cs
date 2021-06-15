using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnManager : MonoBehaviour
{

    public enum SpawnState { Spawning, Waiting, CoolDown };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float spawnRate;
    }
    public Wave[] waves;
    private int nextWave = 0;

    public float coolDownBetweenWaves = 5f;
    private float waveCooldown;
    private float searchCooldown = 1f;
    public SpawnState state = SpawnState.CoolDown;
    [SerializeField] private UIManager _uiManager;




  
    private bool _spawn = true;
    private float _randomWait;
    private int _powerID;


    [Header("Game Objects")]
   // [SerializeField] private GameObject _enemyPreFab;
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
        NullCheck();
        waveCooldown = coolDownBetweenWaves;
        StartCoroutine(SpawnPowerUpRoutine());
        updateUI(waves[nextWave]);


    }
/*
        _spawnDelay = new WaitForSeconds(_delaytime);
        StartCoroutine(SpawnRoutine());
        _spawnDelaybg = new WaitForSeconds(delaytimebg);
        _spawnDelaybgSmall = new WaitForSeconds(delaytimebgSmall);
         StartCoroutine(BackGroundObjects());
        StartCoroutine(SmallBackgroundObjects());
*/

    private void Update()
    {
        if (state == SpawnState.Waiting)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted(); 
                return;
            }else            
                return;
        }
        if (waveCooldown <= 0)
        {
            if (state != SpawnState.Spawning && _spawn)
                StartCoroutine(SpawnWave(waves[nextWave]));
        }else
            waveCooldown -= Time.deltaTime;
        


#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P)) // spawn a random power up, only works in editor
        {
            Vector3 randomX = new Vector3(Random.Range(-7f, 7f), 8, 0);
            _powerID = Random.Range(0, _powerUpContainer.Length);
            Instantiate(_powerUpContainer[_powerID], randomX, Quaternion.identity);
        }
#endif

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
        
/*
    IEnumerator SpawnRoutine()
    {
        while (_spawn == true)
        {
            yield return _spawnDelay;
            Transform newEnemy = Instantiate(_enemy, transform.position, Quaternion.identity);
            newEnemy.parent = _enemyContainer.transform;

        }
    }
*/

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_spawn == true)
        {            
            Vector3 randomX = new Vector3(Random.Range(-7f, 7f), 8, 0);
            if (Random.Range(0,100) > 95)
            {
                Instantiate(_powerUpContainer[5], randomX, Quaternion.identity); // star powerup
                yield break;
            }
            else
            {
            _powerID = Random.Range(0, _powerUpContainer.Length - 1); // can choose any but the last power up (the star)
            Instantiate(_powerUpContainer[_powerID], randomX, Quaternion.identity);
            }
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
//        if (_enemyPreFab == null)
//            Debug.LogError("SpawnManager:Enemy PreFab is null");
        if (_enemyContainer == null)
            Debug.LogError("SpawnManager:Enemy Container is null");
        if (_powerUpContainer == null)
            Debug.LogError("SpawnManager:PowerUp Container is null");

    }

    

    //SpawnSystem

 

    void WaveCompleted()
    {
        if (_spawn)
        {

            Debug.Log("Wave Completed!");
            state = SpawnState.CoolDown; 
            waveCooldown = coolDownBetweenWaves; 
            if (nextWave + 1 > waves.Length - 1)
            {
                nextWave = 0; // loops waves, will add in a Congrats on winnning later.            
            }
            else
            {
                nextWave++;
                updateUI(waves[nextWave]);
            }
        }
    }

    bool EnemyIsAlive()
    {
        searchCooldown -= Time.deltaTime; 
        if (searchCooldown <= 0f) 
        {
            searchCooldown = 1f; 
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0) 
            {
                return false; 
            }
        }
        return true; 
    }

    IEnumerator SpawnWave(Wave _wave)
    {
       
//        Debug.Log("Spawning Wave: " + _wave.name);      
        state = SpawnState.Spawning;                    
        //Spawn
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy); 
            yield return new WaitForSeconds(1f / _wave.spawnRate); 
        }
        state = SpawnState.Waiting;                    
        yield break;
        
    }

    void SpawnEnemy(Transform _enemy)
    {
        // Spawn Enemy
//        Debug.Log("Spawning Enemy: " + _enemy.name);
        Transform newEnemy = Instantiate(_enemy, transform.position, Quaternion.identity);
        newEnemy.parent = _enemyContainer.transform;        
    }

    private void updateUI(Wave _wave)
    {
        _uiManager.UpdateWave(_wave.name);
    }
    



}
